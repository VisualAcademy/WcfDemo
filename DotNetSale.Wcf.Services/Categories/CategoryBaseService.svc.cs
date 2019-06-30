using DotNetSale.Wcf.Models;
using System.Collections.Generic;

namespace DotNetSale.Wcf.Services.Categories
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "CategoryBaseService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 CategoryBaseService.svc나 CategoryBaseService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class CategoryBaseService : IBreadShop<CategoryBase>
    {
        private readonly CategoryBaseRepository _repository;

        public CategoryBaseService()
        {
            _repository = new CategoryBaseRepository();
        }

        public CategoryBase Add(CategoryBase model)
        {
            return _repository.Add(model); 
        }

        public CategoryBase Browse(int id)
        {
            return _repository.Browse(id); 
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id); 
        }

        public bool Edit(CategoryBase model)
        {
            return _repository.Edit(model); 
        }

        public int Has()
        {
            return _repository.Has(); 
        }

        public IEnumerable<CategoryBase> Ordering(OrderOption orderOption)
        {
            return _repository.Ordering(orderOption); 
        }

        public List<CategoryBase> Paging(int pageNumber, int pageSize)
        {
            return _repository.Paging(pageNumber, pageSize);
        }

        public List<CategoryBase> Read()
        {
            return _repository.Read(); 
        }

        public List<CategoryBase> Search(string query)
        {
            return _repository.Search(query); 
        }
    }
}
