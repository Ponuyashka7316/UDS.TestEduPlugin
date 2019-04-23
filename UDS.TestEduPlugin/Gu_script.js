//Rework
//TODO:
/*

*/
//PRACTICE
//set visibility 
function setVisibiliy() {

    var temp = Xrm.Page.getControl("new_enablevisibility").getAttribute().getText();
    if (temp == "true") {
        Xrm.Page.ui.tabs.get("info_tab").setVisible(true);
        Xrm.Page.ui.tabs.get("info_tab").sections.get("tab_2_section_1").setVisible(true);
    }
    else {
        Xrm.Page.ui.tabs.get("info_tab").setVisible(false);
        Xrm.Page.ui.tabs.get("info_tab").sections.get("tab_2_section_1").setVisible(false);
    }
}

//alternative set visibility
function setVisibiliy() {

    var temp = Xrm.Page.getControl("new_enablevisibility2").getAttribute().getValue();
    Xrm.Page.ui.tabs.get("info_tab").setVisible(temp);
    Xrm.Page.ui.tabs.get("info_tab").sections.get("tab_2_section_1").setVisible(temp);
}

//disable section
function sectiondisable(sectionname) {
    var disablestatus = Xrm.Page.getControl("new_setdisabled").getAttribute().getValue();
    var ctrlName = Xrm.Page.ui.controls.get();
    for (var i in ctrlName) {
        var ctrl = ctrlName[i];
        if (ctrl.getParent()) {
            var ctrlSection = ctrl.getParent().getName();
            if (ctrlSection == sectionname) {
                ctrl.setDisabled(disablestatus);
            }
        }
    }
}

//log fields in section
function LogAllFieldsInSection(tabName, sectionName) {
    try {
        // debugger;
        if (Xrm.Page.ui.tabs.get(tabName).sections.get(sectionName) != null && Xrm.Page.ui.tabs.get(tabName).sections.get(sectionName) != undefined) {
            var controls = Xrm.Page.ui.tabs.get(tabName).sections.get(sectionName).controls.get();

            var numberOfControls = controls.length;

            for (var i = 0; i < numberOfControls; i++) {
                var fieldName = controls[i].getName();
                //var type = Xrm.Page.getControl(fieldName).getControlType();
                console.log(fieldName);
            }
        }
    }
    catch (ex) {
        alert("Exception from EnableOrDisableSection Function : " + ex.message);
    }
}

//set fields read only
function SetFieldsReadOnly() {
    var disablestatus = Xrm.Page.getAttribute("new_hideanddisabletextfield").getValue();
    Xrm.Page.getControl("new_textfield").setDisabled(disablestatus);
    Xrm.Page.getAttribute("new_textfield").setSubmitMode("always");
    Xrm.Page.getControl("new_wholenumberfield").setVisible(!disablestatus);
    Xrm.Page.getControl("new_wholenumberfield").setDisabled(disablestatus);
}

//trying to sitch forms
function ChangeForm() {

    var currentForm = Xrm.Page.ui.formSelector.getCurrentItem();
    var forms = Xrm.Page.ui.formSelector.items.get();
    // var numberOfForms = forms.length;
    var fType = Xrm.Page.ui.getFormType();
    var form = forms[1];

    if (currentForm.getLabel().toLowerCase() == "information" && fType == 1) {
        forms[0].navigate();

    }
    if (fType == 2)
        console.log("event info");
    else
        console.log("nothing changed");

}
//TASK 2
//change lookup account view on Gu_Main entity
function ChangeLookUpViewForAccount() {
    var viewId = "F0EE06D5-BB78-465F-BADA-FC3F5CF05300";
    var entityName = "account";
    var viewDisplayName = "Gu Custom View";
    var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
        "<entity name='account'>" +
        "<attribute name='name' />" +
        "<attribute name='accountid' />" +
        "<filter type='and'>" +
        "<condition attribute='emailaddress1' operator='not-null'/>" +
        "</filter>" +
        "</entity>" +
        "</fetch>";

    var layoutXml = "<grid name='resultset' object='1' jump='accountid' select='1' icon='1' preview='1'>" +
        "<row name='result' id='accountid'>" +
        "<cell name='name' width='150' />" +
        "<cell name='emailaddress1' width='150' />" +
        "</row>" +
        "</grid>";
    var isDefault = true;

    Xrm.Page.getControl("new_lookupfield").addCustomView(viewId, entityName, viewDisplayName, fetchXml, layoutXml, isDefault);
    Xrm.Page.getControl("new_lookupfield").setDefaultView(viewId);

}

//set field color
function ChangeColor() {
    var color = Xrm.Page.getAttribute("new_changecolor").getText();
    document.getElementById("new_optionset").style.backgroundColor = color;
    document.getElementById("myprefix_name").style.backgroundColor = color;
    document.getElementById("new_booleanfield").style.backgroundColor = color;
    document.getElementById("new_decimalfield").style.backgroundColor = color;
}

//change lookup contact view on Gu_Main entity
function ChangeLookUpViewForContact() {
    var viewId = "F0EE06D6-BB78-465F-BADA-FC3F5CF05300";
    var entityName = "contact";
    var viewDisplayName = "Gu CustomView";
    var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
        "<entity name='contact'>" +
        "<attribute name='fullname' />" +
        "<attribute name='contactid' />" +
        "<attribute name='new_uds_code' />" +
        "<filter type='and'>" +
        "<condition attribute='new_uds_code' value='5' operator='eq'/>" +
        "</filter>" +
        "</entity>" +
        "</fetch>";

    var layoutXml = "<grid name='resultset' object='1' jump='contactid' select='1' icon='1' preview='1'>" +
        "<row name='result' id='contactid'>" +
        "<cell name='fullname' width='150' />" +
        "<cell name='emailaddress1' width='150' />" +
        "<cell name='new_uds_code' width='50' />" +
        "</row>" +
        "</grid>";
    var isDefault = true;

    Xrm.Page.getControl("new_contact").addCustomView(viewId, entityName, viewDisplayName, fetchXml, layoutXml, isDefault);
    Xrm.Page.getControl("new_contact").setDefaultView(viewId);

}

// set filter to subgrid
function FilterSubGrid() {
    var subgrid = document.getElementById("ContactsForOrg");
    if (subgrid == null || subgrid.length == 0) {
        setTimeout(FilterSubGrid, 2000);
        return;
    }
    var requestId = Xrm.Page.data.entity.getId();
    if (!requestId) {
        return;
    }
    debugger;
    var account = Xrm.Page.getAttribute("new_lookupfield").getValue();
    var accName = "";
    if (account) {
        accName = account[0].id;
    }
 
    var fetchXml =
        "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
        "  <entity name='contact'>" +
        "    <attribute name='fullname' />" +
        "    <attribute name='telephone1' />" +
        "    <attribute name='contactid' />" +
        "    <order attribute='fullname' descending='false' />" +
        "    <filter type='and'>" +
        "      <condition attribute='parentcustomerid' operator='eq' uitype='account' value='" + accName + "' />" +
        "    </filter>" +
        "  </entity>" +
        "</fetch>";
    subgrid.control.SetParameter("fetchxml", fetchXml);
    //Refresh grid to show filtered records only. 
    subgrid.control.Refresh();
}

//LAB 3
//odata request to retrieve account data with associated primary contact data
function UseOData() {
    debugger;
    var req = new XMLHttpRequest();
    req.open("GET", Xrm.Page.context.getClientUrl() + "/api/data/v8.2/accounts?$select=accountid,telephone1&$expand=primarycontactid($select=emailaddress1,emailaddress2,emailaddress3,firstname,fullname)&$filter=_primarycontactid_value ne null", false);
    req.setRequestHeader("OData-MaxVersion", "4.0");
    req.setRequestHeader("OData-Version", "4.0");
    req.setRequestHeader("Accept", "application/json");
    req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    req.setRequestHeader("Prefer", "odata.include-annotations=\"*\"");
    req.onreadystatechange = function () {
        if (this.readyState === 4) {
            req.onreadystatechange = null;
            if (this.status === 200) {
                var results = JSON.parse(this.response);
                for (var i = 0; i < results.value.length; i++) {
                    var accountid = results.value[i]["accountid"];
                    var telephone1 = results.value[i]["telephone1"];
                    //Use @odata.nextLink to query resulting related records
                    var primarycontactid_NextLink = results.value[i]["primarycontactid@odata.nextLink"];
                }
            } else {
                Xrm.Utility.alertDialog(this.statusText);
            }
        }
    };
    req.send();
    var res = req.responseText;


}

//set current contact entity state to deactivate
function SetState() {
    var contactId = Xrm.Page.data.entity.getId();
    XrmServiceToolkit.Soap.SetState("contact", contactId, 1, 2);

};


//give read/write rights to specific user
function GiveRights() {
    var targetUserId = "213DF37D-2351-E911-8117-00155D05FA01";
    var accessOptions = {
        targetEntityName: "contact",
        targetEntityId: contactId,
        principalEntityName: "systemuser",
        principalEntityId: targetUserId,
        accessRights: ["ReadAccess", "WriteAccess"]
    };
    XrmServiceToolkit.Soap.GrantAccess(accessOptions);

}

//current user roles
function GetCurrentUserRoles() {
    var roles = XrmServiceToolkit.Soap.GetCurrentUserRoles();
    for (var item in roles)
        console.log(roles[item] + " ");

}

//get contacts associated with any account where address is not null
function GetAssociatedContacts() {
    var fetchXml =
        "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>" +
        "<entity name='contact'>" +
        "<attribute name='fullname' />" +
        "<attribute name='telephone1' />" +
        "<attribute name='contactid' />" +
        "<order attribute='fullname' descending='false' />" +
        "<link-entity name='account' from='primarycontactid' to='contactid' alias='ad'>" +
        "<filter type='and'>" +
        "<condition attribute='address1_composite' operator='not-null' />" +
        "<condition attribute='createdon' operator='on-or-after' value='2016-04-11' />" +
        "</filter>" +
        "</link-entity>" +
        "</entity>" +
        " </fetch>";

    var retrievedContacts = XrmServiceToolkit.Soap.Fetch(fetchXml, true);
    Xrm.Page.getAttribute("new_associatedcontacts").setValue(" Output data: \n ");
    for (var item in retrievedContacts) {
        var temp = Xrm.Page.getAttribute("new_associatedcontacts").getValue();
        Xrm.Page.getAttribute("new_associatedcontacts").setValue(temp + " " + retrievedContacts[item].id);

    }
}

//LAB 4
// 
function UseAggregateFunctions() {
    var fetchXml =
        "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true' aggregate='true'>" +
        "<entity name='myprefix_gu_main'>" +
        "<attribute name='myprefix_gu_mainid' alias='_count' aggregate='count'/>" +
        "<filter type='and'>" +
        "<condition attribute='myprefix_gu_mainid' operator='eq' value='" + Xrm.Page.data.entity.getId() + "' />" +
        "</filter>" +
        "<link-entity name='new_new_l_myprefix_gu_main' from='myprefix_gu_mainid' to='myprefix_gu_mainid' visible='false' intersect='true'>" +
        "<link-entity name='new_l' from='new_lid' to='new_lid' alias='ab'>" +
        "<filter type='and'>" +
        "<condition attribute='new_moneyfield' operator='not-null' />" +
        "</filter>" +
        "</link-entity>" +
        "</link-entity>" +
        "</entity>" +
        "</fetch>";


    var res = XrmServiceToolkit.Soap.Fetch(fetchXml);
    debugger;
    var count = res[0].attributes["_count"].formattedValue; //- общее количество нужных записей
    Xrm.Page.getAttribute("new_associatedcontacts").setValue(" Output data: \n ");
    Xrm.Page.getAttribute("new_associatedcontacts").setValue(count);
}

function UsingGroupBy() {
    debugger;
    var fetchXml =
        "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' >" +
        "  <entity name='new_l'>" +
        "<order attribute='new_name' descending='false' />" +
        "    <link-entity name='new_new_l_myprefix_gu_main' from='new_lid' to='new_lid' visible='false' intersect='true'>" +
        "      <link-entity name='myprefix_gu_main' from='myprefix_gu_mainid' to='myprefix_gu_mainid' alias='ab'>" +
        "        <filter type='and'>" +
        "          <condition attribute='myprefix_gu_mainid' operator='eq' value='" + Xrm.Page.data.entity.getId() + "' />" +
        "        </filter>" +
        "      </link-entity>" +
        "    </link-entity>" +
        "  </entity>" +
        "</fetch>";


    var res = XrmServiceToolkit.Soap.Fetch(fetchXml);
    debugger;
    console.log(res);
    for (var i in res)
        console.log(res[i].attributes.new_name.value + " ");
}

function UsingGroupingByMoney() {
    debugger;
    var fetchXml =
        "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true' aggregate='true'>" +
        "  <entity name='new_l'>" +
        "    <attribute name='new_moneyfield' alias='estimatedvalue_sum' aggregate='sum'/>" +
        "  <attribute name='new_opt' alias='group' groupby='true'/> " +
        "    <link-entity name='new_new_l_myprefix_gu_main' from='new_lid' to='new_lid' visible='false' intersect='true'>" +
        "      <link-entity name='myprefix_gu_main' from='myprefix_gu_mainid' to='myprefix_gu_mainid' alias='ab'>" +
        "        <filter type='and'>" +
        "          <condition attribute='myprefix_gu_mainid' operator='eq' value='" + Xrm.Page.data.entity.getId() + "' />" +
        "        </filter>" +
        "      </link-entity>" +
        "    </link-entity>" +
        "  </entity>" +
        "</fetch>"


    var res = XrmServiceToolkit.Soap.Fetch(fetchXml);
    debugger;

    console.log(res);
    for (var i in res) {
        console.log(res[i].attributes.group.formattedValue + ": " + res[i].attributes["estimatedvalue_sum"].formattedValue);
    }
}



//trying to change BPF
function ChangeBPF() {
    debugger;
    //var proc = Xrm.Page.data.process.getActiveProcess();
    Xrm.Page.getAttribute("new_changeprocess").setSubmitMode("always");
    var temp = Xrm.Page.getAttribute("new_changeprocess").getValue();
    debugger
    if (temp) {
        Xrm.Page.data.process.setActiveProcess("33D3FF3D-2F14-4AD6-A492-45BECD63EE2E", callBack);  //33d3ff3d-2f14-4ad6-a492-45becd63ee2e
        Xrm.Page.data.entity.save();
        Xrm.Page.data.refresh(true);
    }
    else {
        Xrm.Page.data.process.setActiveProcess("8760EEA2-58B4-4996-ABFB-248461844371", callBack);   //e66a6727-aafb-49de-8109-97000e72869a
        Xrm.Page.data.entity.save();
        Xrm.Page.data.refresh(true);
    }
}

function callBack(response) {
    debugger
    alert("BPF Changing");

}
//next stage
function MoveNext() {
    debugger;
    Xrm.Page.data.process.moveNext();

}

//next stage
function MovePrev() {
    debugger;
    Xrm.Page.data.process.movePrevious();
}

//LAB 5 
//load email from lookup(related entity) to another field
function loadEmail() {

    var cont = Xrm.Page.getAttribute("new_contact").getValue();

    var relatedContact = XrmServiceToolkit.Soap.Retrieve("contact", cont[0].id, ['emailaddress1']);
    if (relatedContact.attributes["emailaddress1"] == null) {
        Xrm.Page.getAttribute("new_textfield").setValue("dddd@gmail.com");
        Xrm.Page.data.entity.save();
        return;
    }
    Xrm.Page.getAttribute("new_textfield").setValue(relatedContact.attributes["emailaddress1"].value);
    Xrm.Page.data.entity.save();
}

//for enable rule in workbench
function hasRole() {
    var roles = XrmServiceToolkit.Soap.GetCurrentUserRoles();
    if (roles.includes('System Administrator') || roles.includes('Customer manager'))
        return true;
    return false;
}

//trying to deactivate selected records
//function deactivateRecords(selectedControl, selectedItems, typeCode) {
function deactivateRecords(control, selectedItems, TypeCode) {


    var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
        "  <entity name='myprefix_gu_main'>" +
        "    <attribute name='myprefix_name' />" +
        "    <attribute name='createdon' />" +
        "    <attribute name='new_decimalfield' />" +
        "    <attribute name='myprefix_gu_mainid' />" +
        "    <order attribute='myprefix_name' descending='false' />" +
        "    <filter type='and'>" +
        "      <condition attribute='new_contact' operator='not-null' />" +
        "    </filter>" +
        "  </entity>" +
        "</fetch>";

    debugger
    var result = XrmServiceToolkit.Soap.Fetch(fetchXml);
    var isLookup = false;

    for (var i = 0; i < result.length; i++) {
        for (var j = 0; j < selectedItems.length; j++) {
            item = selectedItems[j].Id.replace("{", "");
            item = item.replace("}", "");
            item = item.toLowerCase();
            if (result[i].id == item) {
                isLookup = true;
                break;
            }
        }

        if (isLookup) break;
    }

    if (isLookup) {
        if (confirm("Выбранные записи содержат связь с клиентом. Продолжить деактивацию?")) {
            for (var k = 0; k < selectedItems.length; k++) {
                item = selectedItems[k].Id.replace("{", "");
                item = item.replace("}", "");
                item = item.toLowerCase();
                debugger;
                XrmServiceToolkit.Soap.SetState("myprefix_gu_main", item, 1, 2, false);
            }
            window.location.reload(true);

        }
    }
    else {
        for (var l = 0; l < selectedItems.length; l++) {

            debugger;
            item = selectedItems[l].Id.replace("{", "");
            item = item.replace("}", "");
            item = item.toLowerCase();
            XrmServiceToolkit.Soap.SetState("myprefix_gu_main", item, 1, 2, false);
        }
        window.location.reload(true);

        //Mscrm.GridCommandActions.deactivate(control, selectedItems, TypeCode);
    }
}

// <script src="ClientGlobalContext.js.aspx" type="text/javascript"></script>

//disable selected fields
function setFieldsRequired(isDialog = false) {

    var checkBoxs = document.querySelectorAll("input[type=checkbox]:checked");
    for (var i = 0; i < checkBoxs.length; i++) {
        var type = checkBoxs[i].value;
        setAllSelectedControls(type, true, isDialog);
        //parent.Xrm.Page.getAttribute(temp).setRequiredLevel("required");
    }
    close();
}


function setAllSelectedControls(type, state, isDialog) {
    if (!isDialog) {
        parent.Xrm.Page.ui.controls.forEach(function (control, index) {
            var controlType = control.getControlType();
            if (controlType != "iframe" && controlType != "webresource" && controlType != "subgrid") {
                if (controlType == type)
                    control.setDisabled(state);
            }
        });
    }
    else {
        parent.opener.Xrm.Page.ui.controls.forEach(function (control, index) {
            var controlType = control.getControlType();
            if (controlType != "iframe" && controlType != "webresource" && controlType != "subgrid") {
                if (controlType == type)
                    control.setDisabled(state);
            }
        });
    }
}

//disable selected fields
function clearRequired(isDialog = false) {
    var checkBoxs = document.querySelectorAll("input[type=checkbox]:checked");
    for (var i = 0; i < checkBoxs.length; i++) {
        var type = checkBoxs[i].value;
        setAllSelectedControls(type, false, isDialog);
        //parent.opener.Xrm.Page.getAttribute(checkBoxs[i].value).setRequiredLevel("none");
    }
}

//open web res in dialog window
function openHtml() {
    Xrm.Utility.openWebResource("new_Gu_WebRes_2");
}

function GetRequestObject() {
    if (window.XMLHttpRequest) {
        return new window.XMLHttpRequest;
    } else {
        try {
            return new ActiveXObject("MSXML2.XMLHTTP.3.0");
        } catch (ex) {
            return null;
        }
    }
}

function UpdateField() {
    var entityId = Xrm.Page.data.entity.getId();
    debugger;
    var updateEntity = new XrmServiceToolkit.Soap.BusinessEntity("myprefix_gu_main", entityId);
    updateEntity.attributes["new_associatedcontacts"] = "UPDATED!";
    response = XrmServiceToolkit.Soap.Update(updateEntity);
    Xrm.Page.data.entity.save(null);
}

function executeAction() {

    if (confirm("Вы действительно хотите деактивировать все связанные записи Child Entity?")) {
        var testId = Xrm.Page.data.entity.getId();
        item = testId.replace("{", "");
        item = item.replace("}", "");
        item = item.toLowerCase();
        debugger
        Process.callAction(
            "new_Gu_Action",
            [
                { key: "guentity", type: Process.Type.String, value: item },
            ],
            function (params) {
                //var result = params["Random"];
               // alert(result);
            },
            function (error, trace) {
                alert(error);
     
                if (window.console && console.error) {
                    console.error(error + "\n" + trace);
                }
            }
        );
      
        var organizationUrl = Xrm.Page.context.getClientUrl();
/*
        // pass the id as inpurt parameter
        var data = {
            "guentity": item
        };
        debugger
        var query = "new_Gu_Action";
        var req = new XMLHttpRequest();
        req.open("POST", organizationUrl + "/api/data/v8.0/" + query, true);
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.setRequestHeader("OData-MaxVersion", "4.0");
        req.setRequestHeader("OData-Version", "4.0");
        req.onreadystatechange = function () {
            if (this.readyState == 4) {
                req.onreadystatechange = null;
                if (this.status == 200) {
                    var data = JSON.parse(this.response);

                } else {
                    var error = JSON.parse(this.response).error;
                    alert(error.message);
                }
            }
        };
        req.send(window.JSON.stringify(data));
        */
    }
}

// Получаем ответ
// var response = req.responseXML.xml;

function GetClientUrl() {
    if (typeof Xrm.Page.context == "object") {
        clientUrl = Xrm.Page.context.getClientUrl();
    }
    var ServicePath = "/XRMServices/2011/Organization.svc/web";
    return clientUrl + ServicePath;
}

function changeState() {
    debugger;
    if (Xrm.Page.getControl("new_deactivatecontact").getAttribute().getValue() == true)
        XrmServiceToolkit.Soap.SetState("myprefix_gu_main", Xrm.Page.data.entity.getId(), 1, 2, false);
    if (Xrm.Page.getControl("new_deactivatecontact").getAttribute().getValue() == false)
        XrmServiceToolkit.Soap.SetState("myprefix_gu_main", Xrm.Page.data.entity.getId(), 0, 1, false);

    //Save and Reload form
    Xrm.Page.data.refresh(true).then(null, null);
}

//binding  events
function MyMain() {
    Xrm.Page.getAttribute("new_enablevisibility").addOnChange(setVisibiliy);
    //var temp = Xrm.Page.getControl("new_setdisabled").getAttribute().getValue();

    Xrm.Page.getAttribute("new_setdisabled").addOnChange(sectiondisable("tab_2_section_1"));
    LogAllFieldsInSection("info_tab", "tab_2_section_1");
    LogAllFieldsInSection("{0dfd6c18-dbd5-4198-870d-aad6200bacab}", "{6c3090b2-12bc-4652-82cb-801f80f069f8}");
    Xrm.Page.getAttribute("new_hideanddisabletextfield").addOnChange(SetFieldsReadOnly);
    ChangeLookUpViewForAccount();
    ChangeLookUpViewForContact();
    Xrm.Page.getAttribute("new_changecolor").addOnChange(ChangeColor);
    Xrm.Page.getAttribute("new_useodata").addOnChange(UseOData);
    Xrm.Page.getAttribute("new_giverightstoreadwritecontacts").addOnChange(GiveRights);
    Xrm.Page.getAttribute("new_getcurrentuserroles").addOnChange(GetCurrentUserRoles);
    Xrm.Page.getAttribute("new_getassociatedcontacts").addOnChange(GetAssociatedContacts);
    Xrm.Page.getAttribute("new_aggregate").addOnChange(UseAggregateFunctions);
    Xrm.Page.getAttribute("new_nextstage").addOnChange(MoveNext);
    Xrm.Page.getAttribute("new_prevstage").addOnChange(MovePrev);
    Xrm.Page.getAttribute("new_changeprocess").addOnChange(ChangeBPF);
    Xrm.Page.getAttribute("new_updatefield").addOnChange(UpdateField);
    Xrm.Page.getAttribute("new_deactivatecontact").addOnChange(changeState);
    Xrm.Page.getAttribute("new_groupby").addOnChange(UsingGroupBy);
    Xrm.Page.getAttribute("new_orderby").addOnChange(UsingGroupingByMoney);
}

function CreateContact() {
    debugger;
    var createContact = new XrmServiceToolkit.Soap.BusinessEntity("contact");
    createContact.attributes["firstname"] = "Diane";
    createContact.attributes["lastname"] = "Morgan";
    createContact.attributes["middlename"] = "<&>"; // Deliberate special characters to ensure that the toolkit can handle special characters correctly.
    createContact.attributes["gendercode"] = { value: 2, type: "OptionSetValue" };
    createContact.attributes["familystatuscode"] = { value: 1, type: "OptionSetValue" }; // Picklist : Single - 1
    createContact.attributes["creditlimit"] = { value: 2, type: "Money" };
    createContact.attributes["birthdate"] = new Date(98, 1);
    createContact.attributes["donotemail"] = true;
    createContact.attributes["donotphone"] = false;
    createContact.attributes["parentcustomerid"] = { id: "88A2F7DC-7C41-E911-8115-00155D05FA01", logicalName: "account", type: "EntityReference" };
    contactId = XrmServiceToolkit.Soap.Create(createContact);

};



