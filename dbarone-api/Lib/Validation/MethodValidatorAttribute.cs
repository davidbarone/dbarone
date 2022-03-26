﻿namespace dbarone_api.Lib.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

/// <summary>
/// Executes a server method which encapsulates some validation logic
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class MethodValidatorAttribute : ValidatorAttribute
{
    public MethodInfo Method { get; set; }

    public override void DoValidate(object value, object target, string key, IList<ValidationResult> results)
    {
        if (Method == null)
            throw new ArgumentNullException("methodInfo");
        if (Method.ReturnType != typeof(void))
            throw new ArgumentException("MethodValidator method has invalid signature");
        ParameterInfo[] parameters = Method.GetParameters();

        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(IList<ValidationResult>))
        {
            Method.Invoke(target, new object[] { results });
        }
        else
        {
            throw new ArgumentException("MethodValidator method has invalid signature");
        }
    }
}