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

using System.Collections.Immutable;
using System.Reflection;
using System.Xml;
using ExtendedXmlSerialization.Common;
using ExtendedXmlSerialization.Instructions;
using ExtendedXmlSerialization.Write.Services;

namespace ExtendedXmlSerialization.Write.Extensibility
{
    public class CustomSerializationExtension : ConfigurationWritingExtensionBase
    {
        private readonly IInstruction _instruction;

        public CustomSerializationExtension(ISerializationToolsFactory factory, IInstruction instruction)
            : base(factory)
        {
            _instruction = instruction;
        }

        protected override bool StartingMembers(IWriting services, IExtendedXmlSerializerConfig configuration,
                                                object instance,
                                                IImmutableList<MemberInfo> members)
        {
            if (configuration.IsCustomSerializer)
            {
                _instruction.Execute(services);
                configuration.WriteObject(services.Get<XmlWriter>(), instance);
                return false;
            }
            return base.StartingMembers(services, configuration, instance, members);
        }
    }
}