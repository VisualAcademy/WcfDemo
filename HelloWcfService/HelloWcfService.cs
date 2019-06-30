using System.Runtime.Serialization;
using System.ServiceModel;

/// <summary>
/// Visual Studio를 관리자 권한으로 열고 프로젝트를 실행하세요. 
/// </summary>
namespace HelloWcfService
{
    // 모델 클래스
    [DataContract]
    public class SignModel
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }
    }

    // 인터페이스
    [ServiceContract]
    public interface ISignService
    {
        [OperationContract]
        string SignIn(SignModel model);
    }

    // 리포지토리 
    public class SignService : ISignService
    {
        public string SignIn(SignModel model)
        {
            return $"{model.Email} - {model.Password}로 로그인했습니다.";
        }
    }
}
