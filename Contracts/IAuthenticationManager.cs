using ApiApplication.Models;
using System.Threading.Tasks;

namespace ApiApplication.Contracts
{
    public interface IAuthenticationManager
    {

        string CreateToken(Account account);
    }
}
