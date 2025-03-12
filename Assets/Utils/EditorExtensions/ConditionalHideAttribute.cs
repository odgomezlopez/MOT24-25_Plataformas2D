using UnityEngine;

public class ConditionalHideAttribute : PropertyAttribute
{
	public string ConditionalSourceField;

	public ConditionalHideAttribute(string conditionalSourceField)
	{
		this.ConditionalSourceField = conditionalSourceField;
	}
}