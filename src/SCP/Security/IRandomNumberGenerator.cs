using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCP.Security
{
    public interface IRandomNumberGenerator
    {
        int Generate();

        long GenerateLong();
    }
}
