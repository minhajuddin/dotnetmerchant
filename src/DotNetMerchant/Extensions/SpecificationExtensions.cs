#region License

// The MIT License
// 
// Copyright (c) 2009 Conatus Creative, Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

using System;
using DotNetMerchant.Specifications;

namespace DotNetMerchant.Extensions
{
    internal static class SpecificationExtensions
    {
        public static bool Satisfies<T>(this object instance)
            where T : ISpecification
        {
            var marker = Activator.CreateInstance<T>();
            var type = typeof (ISpecification<>).MakeGenericType(instance.GetType());
            var match = marker.Implements(type);

            if (!match)
            {
                return false;
            }

            var method = type.GetMethod("IsSatisfiedBy");
            var result = method.Invoke(marker, new[] {instance});

            return (bool) result;
        }

        public static bool Satisfies(this object instance, ISpecification specificationType)
        {
            var type = typeof(ISpecification<>).MakeGenericType(instance.GetType());
            var match = specificationType.Implements(type);

            if (!match)
            {
                return false;
            }

            var method = type.GetMethod("IsSatisfiedBy");
            var result = method.Invoke(specificationType, new[] { instance });

            return (bool)result;
        }
    }
}