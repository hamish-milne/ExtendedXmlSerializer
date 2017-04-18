// MIT License
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

using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Content.Composite;
using ExtendedXmlSerializer.ContentModel.Content.Composite.Collections;
using ExtendedXmlSerializer.ContentModel.Content.Composite.Members;
using ExtendedXmlSerializer.ContentModel.Identification;
using ExtendedXmlSerializer.ContentModel.Reflection;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Sources;
using JetBrains.Annotations;

namespace ExtendedXmlSerializer.ExtensionModel.Content
{
	sealed class ContentModelExtension : ISerializerExtension
	{
		public static ContentModelExtension Default { get; } = new ContentModelExtension();
		ContentModelExtension() {}

		public IServiceRepository Get(IServiceRepository parameter)
			=> parameter.Register(typeof(IAlteration<>), typeof(DefaultAlterationRegistration<>))
			            /*.Register<IComparer<IContentOption>, SortComparer<IContentOption>>()
			            .RegisterAsSet<IContentOption, ContentOptions>()
			            .RegisterAsStart<IContentOption, Start>()
			            .RegisterAsFinish<IContentOption, Finish>()*/
			            /*.Register<IDictionaryEntries, DictionaryEntries>()*/
			            /*.Register<ArrayContentOption>()*/
			            /*.Register<DictionaryContentOption>()*/
			            /*.Register<CollectionContentOption>()*/
			            .Register<IClassification, Classification>()
			            .Register<IIdentityStore, IdentityStore>()
			            .Register(typeof(IInnerContents<>), typeof(InnerContents<>))
			            .Register<ICompositeContents, CompositeContents>()
			            .Register<ICompositeCollectionContents, CompositeCollectionContents>()
			            .Register(typeof(IMemberHandler<>), typeof(MemberHandler<>))
			            .Register(typeof(IMemberAssignment<>), typeof(MemberAssignmentRegistration<>))
			            .Register(typeof(IContentsHandler<>), typeof(ContentsHandler<>))
			            /*.Register<IDictionaryContents, DictionaryContents>()*/
			            /*.RegisterInstance<IInnerContentResult>(InnerContentResult.Default)
			            .RegisterInstance<ICollectionAssignment>(CollectionAssignment.Default)
			            .RegisterInstance<IListContentsSpecification>(ListContentsSpecification.Default)*/
			            .RegisterInstance(ReflectionSerializer.Default);

		void ICommand<IServices>.Execute(IServices parameter)
		{
			/*parameter.Get<ISortOrder>()
					 .Sort<ArrayContentOption>(0)
					 /*.Sort<DictionaryContentOption>(1)#1#
					 .Sort<CollectionContentOption>(2);*/
		}

		sealed class MemberAssignmentRegistration<T> : IMemberAssignment<T>
		{
			readonly IMemberAssignment<T> _assignment;

			[UsedImplicitly]
			public MemberAssignmentRegistration() : this(MemberAssignment<T>.Default) {}

			public MemberAssignmentRegistration(IMemberAssignment<T> assignment)
			{
				_assignment = assignment;
			}

			public void Assign(IInnerContent<T> contents, IMemberAccess access, object value)
				=> _assignment.Assign(contents, access, value);
		}

/*
		sealed class InnerContentResultRegistration<T> : IInnerContentResult<T>, IInnerContentResult
		{
			readonly IInnerContentResult<T> _result;

			[UsedImplicitly]
			public InnerContentResultRegistration() : this(InnerContentResult<T>.Default) { }

			public InnerContentResultRegistration(IInnerContentResult<T> result)
			{
				_result = result;
			}

			public T Get(IInnerContent parameter)
			{
				return _result.Get(parameter);
			}

			object IParameterizedSource<IInnerContent, object>.Get(IInnerContent parameter)
			{
				throw new InvalidOperationException("This exists for static type checking purposes only.");
			}
		}
*/

		sealed class DefaultAlterationRegistration<T> : IAlteration<T>
		{
			readonly IAlteration<T> _alteration;

			[UsedImplicitly]
			public DefaultAlterationRegistration() : this(Self<T>.Default) {}

			public DefaultAlterationRegistration(IAlteration<T> alteration)
			{
				_alteration = alteration;
			}

			public T Get(T parameter) => _alteration.Get(parameter);
		}
	}
}