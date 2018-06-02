using GQService.com.gq.log;
using GQService.com.gq.security;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

/// <summary>
/// https://jwt.io/
/// </summary>
/// 
namespace GQService.com.gq.jwt
{
    public static class JWTUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static string GenerateToken(object obj, string secretKey)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(signingCredentials);

            //DateTime centuryBegin = new DateTime(1970, 1, 1);
            //var exp = new TimeSpan(DateTime.Now.AddSeconds(30).Ticks - centuryBegin.Ticks).TotalSeconds;
            
            var payload = new JwtPayload("", "", new List<Claim>(), null, new DateTime(DateTime.Now.Ticks + TimeSpan.FromMinutes(500).Ticks));


            var properties = obj.GetType().GetProperties();

            foreach (var prop in properties)
            {
                payload.Add(prop.Name, prop.GetValue(obj));
            }

            var secToken = new JwtSecurityToken(header, payload);

            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(secToken);
            return tokenString;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        public static T GetPayload<T>(string token, string secretKey) where T : class, new()
        {
            T returnObject = null;

            try
            {
                var handler = new JwtSecurityTokenHandler();

                var validationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateActor = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken St;

                var result = handler.ValidateToken(token, validationParameters, out St);

                var tokenS = St as JwtSecurityToken;

                var payload = tokenS.Payload;

                returnObject = (T)Activator.CreateInstance(typeof(T));

                var properties = returnObject.GetType().GetProperties();

                foreach (var prop in properties)
                {
                    if (payload.ContainsKey(prop.Name))
                    {
                        prop.SetValue(returnObject, payload[prop.Name]);
                    }
                }
            }
            catch (SecurityTokenExpiredException ex)
            {
                Security.TimeExpired = true;
            }
            catch (Exception ex)
            {
                Log.Error("JWTUtil - GetPayload", ex);

            }

            return returnObject;
        }

        public static T GetPayloadSinControl<T>(string token, string secretKey) where T : class, new()
        {
            T returnObject = null;

            try
            {
                var handler = new JwtSecurityTokenHandler();

                var validationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    ValidateActor = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true
                };

                SecurityToken St;

                var result = handler.ValidateToken(token, validationParameters, out St);

                var tokenS = St as JwtSecurityToken;

                var payload = tokenS.Payload;

                returnObject = (T)Activator.CreateInstance(typeof(T));

                var properties = returnObject.GetType().GetProperties();

                foreach (var prop in properties)
                {
                    if (payload.ContainsKey(prop.Name))
                    {
                        prop.SetValue(returnObject, payload[prop.Name]);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("JWTUtil - GetPayload", ex);

            }

            return returnObject;
        }


    }

   

}
