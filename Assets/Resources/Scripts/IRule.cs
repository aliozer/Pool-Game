using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolGame.Rules
{
    public interface IRule<ReturnType>
    {
        ReturnType Execute();
    }

    public interface IRule
    {
        void Execute();
    }
}
