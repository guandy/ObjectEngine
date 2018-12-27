/***************************************************************************
*author:guandy / 258631664@qq.com
* git:https://github.com/guandy/ 
*createtime:2018-12-11 15:00:41
*updatetime:2018-12-11 15:00:41
***************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ObjectEngine
{
    /// <summary>
    /// 对象引擎
    /// </summary>
    public class ObjectEngine
    {
        private readonly string FirstPathName = "data";

        public Hashtable Hashtable { get; set; }

        public bool IsInitData { get; set; }

        public void SetData<T>(T obj) where T : class
        {
            this.Hashtable = ToHashtable(obj);
            this.IsInitData = true;
        }
        /// <summary>
        /// 根据标签获取值 例：data.Name 或 data.UserList[0].Name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public  object GetValue(string label)
        {
            if (!this.IsInitData)
                throw new Exception("请先用SetData赋值");
            if (this.Hashtable == null)
                return null;
            var labelArry = label.Split('.');
            var labelIndex = 0;
            object labelValue = this.Hashtable;
            while (labelIndex < labelArry.Count())
            {
                var isList = false;
                var listIndex = 0;
                var labelName = labelArry[labelIndex];
                if (labelName.Contains("["))
                {
                    isList = true;
                    var labelNameArry = labelName.Split('[');
                    if (labelNameArry.Length != 2)
                    {
                        throw new Exception($"{labelName}格式不正确");
                    }
                    if (!int.TryParse(labelNameArry[1].Replace("]", ""),out listIndex))
                    {
                        throw new Exception($"{labelName}格式不正确,索引必须是数字");
                    }
                    labelName = labelNameArry[0];
                }
                var labelHashtable = labelValue as Hashtable;
                if (!labelHashtable.ContainsKey(labelName))
                    throw new Exception($"数据不包含属性{labelName}");
                if (!isList)
                {
                    labelValue = labelHashtable[labelName];
                }
                else
                {
                    labelValue = labelHashtable[labelName];
                    if (labelValue is IList)
                    {
                        var list = labelValue as IList;
                        labelValue = list[listIndex];
                    }
                }
                
                labelIndex++;
            }
            return labelValue;
        }
        /// <summary>
        /// 将T转换为Hashtable列表
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private  Hashtable ToHashtable<T>(T entity,bool isFirst = true) where T : class
        {
            Hashtable ht = new Hashtable();
            if (entity == null)
                return ht;
            var propertyInfoList = entity.GetType().GetProperties();
            
            foreach (var item in propertyInfoList)
            {
                var name = item.Name;
                var propertyType = item.PropertyType;
                var value = item.GetValue(entity);
                object resultValue;
                if (IsValuePro(propertyType,value,out resultValue))
                {
                    ht[name] = resultValue;
                }
                else if (propertyType.FullName.Contains("System.Collections.Generic.List"))
                {
                    if (value == null)
                    {
                        ht[name] = ListToHashtable(null);
                    }
                    else
                    {
                        var valueList = value as IList;
                        List<object> objList = new List<object>();
                        foreach (var item2 in valueList)
                        {
                            objList.Add(item2);
                        }
                        ht[name] = ListToHashtable(objList);
                    }
                }
                else
                {
                    ht[name] = ToHashtable(value,false);
                }
            }
            if(!isFirst)
                return ht;
            Hashtable result = new Hashtable();
            result[FirstPathName] = ht;
            return result;
        }


        private  List<Hashtable> ListToHashtable(IList<object> objList)
        {
            var list = new List<Hashtable>();
            if (objList == null)
                return list;
            foreach (var entity in objList)
            {
                var item = ToHashtable(entity,false);
                list.Add(item);
            }
            return list;
        }

        private  bool IsValuePro(Type type,object proValue, out object value)
        {
            if (type.FullName.Contains("System.String"))
            {
                value = proValue.ToStr("");
                return true;
            }
            else if (type.FullName.Contains("System.Int32"))
            {
                value = proValue.ToIntNull();
                return true;
            }
            else if (type.FullName.Contains("System.Decimal"))
            {
                value = proValue.ToDecimalNull();
                return true;
            }
            else if (type.FullName.Contains("System.Double"))
            {
                value = proValue.ToDouble(0);
                return true;
            }
            else if (type.FullName.Contains("System.DateTime"))
            {
                value = proValue.ToDateNull();
                return true;
            }
            else if (type.FullName.Contains("System.Boolean"))
            {
                value = proValue.ToStr("");
                return true;
            }
            else if (type.FullName.Contains("System.Long"))
            {
                value = proValue.ToDouble(0);
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }


    }
}
