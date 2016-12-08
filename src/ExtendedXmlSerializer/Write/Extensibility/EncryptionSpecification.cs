﻿// MIT License
// 
// Copyright (c) 2016 Wojciech Nagórski
//                    Michael DeMond
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using ExtendedXmlSerialization.Specifications;
using ExtendedXmlSerialization.Write.Services;

namespace ExtendedXmlSerialization.Write.Extensibility
{
    class EncryptionSpecification : ISpecification<object>
    {
        private readonly ISerializationToolsFactory _factory;
        private readonly IWritingContext _context;

        public EncryptionSpecification(ISerializationToolsFactory factory, IWritingContext context)
        {
            _factory = factory;
            _context = context;
        }

        public bool IsSatisfiedBy(object parameter)
        {
            var context = _context.GetMemberContext();
            if (context != null)
            {
                var configuration = _factory.GetConfiguration(context?.Instance.GetType());
                if (configuration != null)
                {
                    var member = context?.Member?.Metadata;
                    if (member != null)
                    {
                        var result =
                            configuration.CheckPropertyEncryption(member.Name);
                        return result;
                    }
                }
            }
            return false;
        }
    }
}