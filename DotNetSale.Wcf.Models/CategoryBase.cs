using System.Runtime.Serialization;

namespace DotNetSale.Wcf.Models
{
    /// <summary>
    /// 카테고리 모델 클래스
    /// </summary>
    [DataContract]
    public class CategoryBase
    {
        /// <summary>
        /// 카테고리 고유 일련번호
        /// </summary>
        [DataMember]
        public int CategoryId { get; set; }

        /// <summary>
        /// 카테고리 이름
        /// </summary>
        [DataMember]
        public string CategoryName { get; set; }
    }
}
