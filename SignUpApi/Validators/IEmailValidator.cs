using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignUpApi.Validators
{
    public interface IEmailValidator
    {
        bool Validate(string email);
    }
}
