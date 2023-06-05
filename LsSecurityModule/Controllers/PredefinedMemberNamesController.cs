using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Security.Strategy;
// ...
public class PredefinedMemberNamesController :
    ObjectViewController<DetailView, SecuritySystemMemberPermissionsObject>
{
    protected override void OnActivated()
    {
        base.OnActivated();
        foreach (PropertyEditor editor in View.GetItems<PropertyEditor>())
        {
            if (editor.PropertyName == "Members")
            {
                ITypeInfo ownerClassInfo = XafTypesInfo.Instance.FindTypeInfo(ViewCurrentObject.Owner.TargetType);
                List<string> propertyNames = new List<string>();
                foreach (IMemberInfo memberInfo in ownerClassInfo.Members)
                    if (/*memberInfo.IsVisible && */memberInfo.IsProperty && memberInfo.IsPublic)
                        propertyNames.Add(memberInfo.Name);
                propertyNames.Sort();
                editor.Model.PredefinedValues = string.Join(";", propertyNames.ToArray());
                break;
            }
        }
    }
}
