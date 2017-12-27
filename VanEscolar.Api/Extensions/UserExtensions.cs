using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VanEscolar.Api.Extensions
{
    public static class UserExtensions
    {
        public static Guid? GetSubject(this ClaimsPrincipal user)
        {
            if (user.HasClaim(c => c.Type.Equals(ClaimTypes.NameIdentifier)))
            {
                var claim = user.Claims.First(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;

                if(!Guid.TryParse(claim, out Guid result))
                {
                    throw new Exception($"Não foi possível converter {claim} para um GUID");
                }
            }

            return null;
        }
    }
}
