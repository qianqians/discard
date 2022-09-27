using abelkhan.admin;

namespace http_admin
{
    [Controller("/transaction")]
    public class TransactionController : ActionSupport
    {
        [Post("/list")]
        public JSON GetList()
        {
            UserList list = new UserList();
            return Result<UserList>.Res(list).ToJson();
        }
    }
}
