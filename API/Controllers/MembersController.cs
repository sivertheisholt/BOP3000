using API.Interfaces.IRepositories;

namespace API.Controllers
{

    /// <summary>
    /// MemberController contains all the endpoints for actions related to a Member
    /// </summary>
    public class MemberController : BaseApiController
    {
        private readonly IMemberRepository _memberRepository;
        public MemberController(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }
    }
}