using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

namespace OKHOSTING.Core.Data.Validation
{
	/// <summary>
	/// Validates a specific MemberMap. CHild classes will be able to perform a validation on a specific MemberMap
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public abstract class MemberValidator : ValidatorBase
	{
		public MemberValidator()
		{
		}

		public MemberValidator(MemberExpression member)
		{
			Member = member;
		}

		/// <summary>
		/// MemberMap that implements the DataValueValidator
		/// </summary>
		public MemberExpression Member { get; set; }
	}
}