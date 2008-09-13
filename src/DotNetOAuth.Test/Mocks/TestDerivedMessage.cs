﻿//-----------------------------------------------------------------------
// <copyright file="TestDerivedMessage.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOAuth.Test.Mocks {
	using System.Runtime.Serialization;

	[DataContract(Namespace = Protocol.DataContractNamespaceV10)]
	internal class TestDerivedMessage : TestBaseMessage {
		/// <summary>
		/// Gets or sets the first value.
		/// </summary>
		/// <remarks>
		/// This element should appear AFTER <see cref="SecondDerivedElement"/>
		/// due to alphabetical ordering rules, but after all the elements in the
		/// base class due to inheritance rules.
		/// </remarks>
		[DataMember]
		public string TheFirstDerivedElement { get; set; }

		/// <summary>
		/// Gets or sets the second value.
		/// </summary>
		/// <remarks>
		/// This element should appear BEFORE <see cref="TheFirstDerivedElement"/>,
		/// but after all the elements in the base class.
		/// </remarks>
		[DataMember]
		public string SecondDerivedElement { get; set; }
	}
}
