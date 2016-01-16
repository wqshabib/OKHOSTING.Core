using System;

namespace OKHOSTING.Data.Validation
{
	/// <summary>
	/// Base class for comparison validators
	/// </summary>
	public abstract class CompareValidator: ValidatorBase
	{
		public CompareValidator()
		{
		}

		public CompareValidator(CompareOperator _operator)
		{
			Operator = _operator;
		}

		/// <summary>
		/// Operator to use in the validation 
		/// </summary>
		public CompareOperator Operator { get; set; }

		/// <summary>
		/// Compare the value of the associated MemberExpression with the 
		/// specified value and using the indicated operator, returns
		/// an ValidationError if the validation fails, or null if it's success
		/// </summary>
		/// <param name="valueToCompare">
		/// Value for comparison
		/// </param>
		/// <returns>
		/// ValidationError if the validation fails, or null if it's success
		/// </returns>
		protected ValidationError Validate(IComparable obj, IComparable valueToCompare)
		{
			//Local Vars
			ValidationError error = null;

			//Validating if the valueToCompare is null
			if (valueToCompare == null) throw new ArgumentNullException("valueToCompare");

			//Loading the value of associated MemberExpression and comparing with the specified value
			int compareResult = obj.CompareTo(valueToCompare);
			
			//Perform the validation in function of the established operator
			switch(this.Operator)
			{
				case CompareOperator.Equal:
					if(compareResult != 0)
						error = new ValidationError(this, "Value must be equal than " + valueToCompare);
					break;
				
				case CompareOperator.NotEqual:
					if(compareResult == 0)
						error = new ValidationError(this, "Value must be different than " + valueToCompare);
					break;

				case CompareOperator.GreaterThan:
					if(compareResult <= 0)
						error = new ValidationError(this, "Value must be greater than than " + valueToCompare);
					break;

				case CompareOperator.GreaterThanEqual:
					if(compareResult < 0)
						error = new ValidationError(this, "Value must be greater or equal than " + valueToCompare);
					break;

				case CompareOperator.LessThan:
					if(compareResult >= 0)
						error = new ValidationError(this, "Value must be less than " + valueToCompare);
					break;

				case CompareOperator.LessThanEqual:
					if(compareResult > 0)
						error = new ValidationError(this, "Value must be less or equal than " + valueToCompare);
					break;
			}
			
			return error;
		}
	}
}