
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery.validate.unobtrusive.js" />



$.validator.unobtrusive.adapters.add(
    'requiredif',
    ['dependentproperty', 'targetvalue'], 
    function (options) {
        options.rules['requiredif'] = {
            dependentproperty: options.params['dependentproperty'],
            targetvalue: options.params['targetvalue']
        };
        options.messages['requiredif'] = options.message;
    });

$.validator.addMethod('requiredif', function (value, element, parameters) {
    var desiredvalue = parameters.targetvalue;
    desiredvalue = (desiredvalue == null ? '' : desiredvalue).toString();
    var controlType = $("input[id$='" + parameters.dependentproperty + "']").attr("type");
    var actualvalue = {}
    if (controlType == "checkbox" || controlType == "radio") {
        var control = $("input[id$='" + parameters.dependentproperty + "']:checked");
        actualvalue = control.val();
    } else {
        actualvalue = $("#" + parameters.dependentproperty).val();
    }
    if ($.trim(desiredvalue).toLowerCase() === $.trim(actualvalue).toLocaleLowerCase()) {
        var isValid = $.validator.methods.required.call(this, value, element, parameters);
        return isValid;
    }
    return true;
});
