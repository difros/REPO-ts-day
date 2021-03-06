﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

/// <summary>
/// https://jwt.io/
/// </summary>
/// 
namespace GQ.Security.JWT
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

            var payload = new JwtPayload();

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
                    ValidateLifetime = false,
                    ValidateActor = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true
                };

                SecurityToken St;

                var result = handler.ValidateToken(token, validationParameters, out St);

                var tokenS = St as JwtSecurityToken;

                var payload = tokenS.Payload;

                returnObject = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(payload));

            }
            catch (Exception ex)
            {
                Log.Log.GetLog().Error("JWTUtil - GetPayload", ex);
            }

            return returnObject;
        }
    }
}
