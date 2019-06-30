using System.Collections.Generic;
using System.ServiceModel;

/// <summary>
/// Development Utility Library
/// </summary>
namespace DotNetSale.Wcf.Models
{
    /// <summary>
    /// 제네릭 인터페이스: 공통 CRUD 코드 => BREAD SHOP 패턴 사용
    /// </summary>
    /// <typeparam name="T">모델 클래스</typeparam>
    [ServiceContract]
    public interface IBreadShop<T> where T : class
    {
        /// <summary>
        /// 상세
        /// </summary>
        [OperationContract]
        T Browse(int id);

        /// <summary>
        /// 출력
        /// </summary>
        [OperationContract]
        List<T> Read();

        /// <summary>
        /// 수정
        /// </summary>
        [OperationContract]
        bool Edit(T model);

        /// <summary>
        /// 입력
        /// </summary>
        [OperationContract]
        T Add(T model);

        /// <summary>
        /// 삭제
        /// </summary>
        [OperationContract]
        bool Delete(int id);

        /// <summary>
        /// 검색
        /// </summary>
        [OperationContract]
        List<T> Search(string query);

        /// <summary>
        /// 건수
        /// </summary>
        [OperationContract]
        int Has();

        /// <summary>
        /// 정렬
        /// </summary>
        [OperationContract]
        IEnumerable<T> Ordering(OrderOption orderOption);

        /// <summary>
        /// 페이징
        /// </summary>
        [OperationContract]
        List<T> Paging(int pageNumber, int pageSize);
    }
}
