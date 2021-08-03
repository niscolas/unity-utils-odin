using System.Collections.Generic;
using System.Linq;
using Plugins.OdinUtils.Editor.Attributes;
using Plugins.OdinUtils.Editor.Validators.AttributeValidators;
using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine;

[assembly: RegisterValidator(typeof(NotEmptyCollectionValidator<>))]

namespace Plugins.OdinUtils.Editor.Validators.AttributeValidators {
	public class NotEmptyCollectionValidator<T> : AttributeValidator<NotEmptyAttribute, T> where T : IEnumerable<Object> {
		protected override void Validate(ValidationResult result) {
			T smartValue = ValueEntry.SmartValue;
			if (smartValue != null && smartValue.Any()) {
				if (smartValue.All(value => !value)) {
					SetResult(result);
				}
			}
			else {
				SetResult(result);
			}
		}

		private static void SetResult(ValidationResult result) {
			result.ResultType = ValidationResultType.Error;
			result.Message = "The collection cannot be empty or have null values ;(";
		}
	}
}