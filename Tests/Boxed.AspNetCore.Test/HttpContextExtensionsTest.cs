namespace Boxed.AspNetCore.Test
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;
    using Xunit;

    public class HttpContextExtensionsTest
    {
        public static IEnumerable<object[]> CacheControlData
        {
            get
            {
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 0,
                        Location = ResponseCacheLocation.Any,
                        NoStore = true,
                        VaryByHeader = null
                    },
                    "no-store"
                };

                // If no-store is set, then location is ignored.
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 0,
                        Location = ResponseCacheLocation.Client,
                        NoStore = true,
                        VaryByHeader = null
                    },
                    "no-store"
                };
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 0,
                        Location = ResponseCacheLocation.Any,
                        NoStore = true,
                        VaryByHeader = null
                    },
                    "no-store"
                };

                // If no-store is set, then duration is ignored.
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 100,
                        Location = ResponseCacheLocation.Any,
                        NoStore = true,
                        VaryByHeader = null
                    },
                    "no-store"
                };

                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 10,
                        Location = ResponseCacheLocation.Client,
                        NoStore = false,
                        VaryByHeader = null
                    },
                    "private,max-age=10"
                };
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 10,
                        Location = ResponseCacheLocation.Any,
                        NoStore = false,
                        VaryByHeader = null
                    },
                    "public,max-age=10"
                };
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 10,
                        Location = ResponseCacheLocation.None,
                        NoStore = false,
                        VaryByHeader = null
                    },
                    "no-cache,max-age=10"
                };
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 365,
                        Location = ResponseCacheLocation.Any,
                        NoStore = false,
                        VaryByHeader = null
                    },
                    "public,max-age=365"
                };
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 20,
                        Location = ResponseCacheLocation.Any,
                        NoStore = false,
                        VaryByHeader = null
                    },
                    "public,max-age=20"
                };
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 1400,
                        Location = ResponseCacheLocation.Any,
                        NoStore = false,
                        VaryByHeader = null
                    },
                    "public,max-age=1400"
                };
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 1600,
                        Location = ResponseCacheLocation.Any,
                        NoStore = false,
                        VaryByHeader = null
                    },
                    "public,max-age=1600"
                };
            }
        }

        public static IEnumerable<object[]> NoStoreData
        {
            get
            {
                // If no-store is set, then location is ignored.
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 0,
                        Location = ResponseCacheLocation.Client,
                        NoStore = true,
                        VaryByHeader = null
                    },
                    "no-store"
                };
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 0,
                        Location = ResponseCacheLocation.Any,
                        NoStore = true,
                        VaryByHeader = null
                    },
                    "no-store"
                };

                // If no-store is set, then duration is ignored.
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 100,
                        Location = ResponseCacheLocation.Any,
                        NoStore = true,
                        VaryByHeader = null
                    },
                    "no-store"
                };
            }
        }

        public static IEnumerable<object[]> VaryData
        {
            get
            {
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 10,
                        Location = ResponseCacheLocation.Any,
                        NoStore = false,
                        VaryByHeader = "Accept"
                    },
                    "Accept",
                    "public,max-age=10"
                };
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 0,
                        Location = ResponseCacheLocation.Any,
                        NoStore = true,
                        VaryByHeader = "Accept"
                    },
                    "Accept",
                    "no-store"
                };
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 10,
                        Location = ResponseCacheLocation.Client,
                        NoStore = false,
                        VaryByHeader = "Accept"
                    },
                    "Accept",
                    "private,max-age=10"
                };
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 10,
                        Location = ResponseCacheLocation.Client,
                        NoStore = false,
                        VaryByHeader = "Test"
                    },
                    "Test",
                    "private,max-age=10"
                };
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 365,
                        Location = ResponseCacheLocation.Any,
                        NoStore = false,
                        VaryByHeader = "Test"
                    },
                    "Test",
                    "public,max-age=365"
                };
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 1400,
                        Location = ResponseCacheLocation.Any,
                        NoStore = false,
                        VaryByHeader = "Test"
                    },
                    "Test",
                    "public,max-age=1400"
                };
                yield return new object[]
                {
                    new CacheProfile
                    {
                        Duration = 1600,
                        Location = ResponseCacheLocation.Any,
                        NoStore = false,
                        VaryByHeader = "Test"
                    },
                    "Test",
                    "public,max-age=1600"
                };
            }
        }

        [Theory]
        [MemberData(nameof(CacheControlData))]
        public void ApplyCacheProfile_CanSetCacheControlHeaders_CacheControlAndPragmaSetToOutput(
            CacheProfile cacheProfile,
            string output)
        {
            var context = new DefaultHttpContext();

            context.ApplyCacheProfile(cacheProfile);

            Assert.Equal(output, context.Response.Headers[HeaderNames.CacheControl]);
        }

        [Theory]
        [MemberData(nameof(NoStoreData))]
        public void ApplyCacheProfile_DoesNotSetLocationOrDuration_IfNoStoreIsSet(
            CacheProfile cacheProfile,
            string output)
        {
            var context = new DefaultHttpContext();

            context.ApplyCacheProfile(cacheProfile);

            Assert.Equal(output, context.Response.Headers[HeaderNames.CacheControl]);
        }

        [Theory]
        [MemberData(nameof(VaryData))]
        public void ApplyCacheProfile_ResponseCacheCanSetVary_CacheControlAndPragmaSetToOutput(
            CacheProfile cacheProfile,
            string varyOutput,
            string cacheControlOutput)
        {
            var context = new DefaultHttpContext();

            context.ApplyCacheProfile(cacheProfile);

            Assert.Equal(varyOutput, context.Response.Headers[HeaderNames.Vary]);
            Assert.Equal(cacheControlOutput, context.Response.Headers[HeaderNames.CacheControl]);
        }

        [Fact]
        public void ApplyCacheProfile_SetsPragmaOnNoCache_CacheControlSetTonoStoreNoCachePragmaSetToNoCache()
        {
            var cacheProfile = new CacheProfile
            {
                Duration = 0,
                Location = ResponseCacheLocation.None,
                NoStore = true,
                VaryByHeader = null
            };
            var context = new DefaultHttpContext();

            context.ApplyCacheProfile(cacheProfile);

            Assert.Equal("no-store,no-cache", context.Response.Headers[HeaderNames.CacheControl]);
            Assert.Equal("no-cache", context.Response.Headers[HeaderNames.Pragma]);
        }
    }
}
