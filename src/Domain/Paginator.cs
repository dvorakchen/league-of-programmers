using System;
using System.Collections.Generic;

namespace Domain
{
    public class Paginator
    {
        public static Paginator New(int index, int size, int capacity = 0)
        {
            return new Paginator
            {
                Index = index <= 0 ? 1 : index,
                Size = size <= 0 ? DEFAULT_SIZE : size,
                _params = new Dictionary<string, string>(capacity)
            };
        }
        /// <summary>
        /// 获取或设置参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get => _params[key];
            set => _params[key] = value;
        }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 行数
        /// </summary>
        public int Size { get; set; } = DEFAULT_SIZE;
        /// <summary>
        /// 每页默认页数
        /// </summary>
        [NonSerialized, Newtonsoft.Json.JsonIgnore]
        public const int DEFAULT_SIZE = 20;
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get
            {
                if (TotalRows % Size == 0)
                    return TotalRows / Size;
                return TotalRows / Size + 1;
            }
        }
        /// <summary>
        /// 总数据数
        /// </summary>
        public int TotalRows { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        [NonSerialized, Newtonsoft.Json.JsonIgnore]
        private Dictionary<string, string> _params;
        /// <summary>
        /// 获取跳过的条数
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int Skip => Size * (Index - 1);
        /// <summary>
        /// 返回数据列表
        /// </summary>
        public dynamic List { get; set; }
        public List<T> GetList<T>()
        {
            return List;
        }
    }
}
