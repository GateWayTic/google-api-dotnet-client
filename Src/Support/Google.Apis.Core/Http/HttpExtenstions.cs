﻿/*
Copyright 2013 Google Inc

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using Google.Apis.Core.Http;
using System.Net;
using System.Net.Http;

namespace Google.Apis.Http
{
    /// <summary>
    /// Extension methods to <see cref="System.Net.Http.HttpRequestMessage"/> and 
    /// <see cref="System.Net.Http.HttpResponseMessage"/>.
    /// </summary>
    public static class HttpExtenstions
    {
        /// <summary>Returns <c>true</c> if the response contains one of the redirect status codes.</summary>
        internal static bool IsRedirectStatusCode(this HttpResponseMessage message)
        {
            switch (message.StatusCode)
            {
                case HttpStatusCode.Moved:
                case HttpStatusCode.Redirect:
                case HttpStatusCode.RedirectMethod:
                case HttpStatusCode.TemporaryRedirect:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>A Google.Apis utility method for setting an empty HTTP content.</summary>
        public static HttpContent SetEmptyContent(this HttpRequestMessage request)
        {
            request.Content = new ByteArrayContent(new byte[0]);
            request.Content.Headers.ContentLength = 0;
            return request.Content;
        }

        /// <summary>
        /// Creates a <see cref="DelegatingHandler"/> which applies the execution interceptor
        /// in <paramref name="interceptor"/> (usually a credential) before delegating onwards.
        /// </summary>
        /// <param name="interceptor">The interceptor to execute before the remainder of the handler chain. Must not be null.</param>
        /// <param name="innerHandler">The initial inner handler.</param>
        /// <returns>A delegating HTTP message handler.</returns>
        public static DelegatingHandler ToDelegatingHandler(this IHttpExecuteInterceptor interceptor, HttpMessageHandler innerHandler) =>
            new InterceptorDelegatingHandler(interceptor, innerHandler);

        /// <summary>
        /// Creates a <see cref="DelegatingHandler"/> which applies the execution interceptor
        /// in <paramref name="interceptor"/> (usually a credential) before delegating onwards.
        /// The <see cref="DelegatingHandler.InnerHandler"/> of the returned handler must be specified before
        /// the first request is made.
        /// </summary>
        /// <param name="interceptor">The interceptor to execute before the remainder of the handler chain. Must not be null.</param>
        /// <returns>A delegating HTTP message handler.</returns>
        public static DelegatingHandler ToDelegatingHandler(this IHttpExecuteInterceptor interceptor) =>
            new InterceptorDelegatingHandler(interceptor);
    }
}
