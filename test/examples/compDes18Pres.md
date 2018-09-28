---
customTheme : "myTheme"
transition: "none"
#highlightTheme: "revitHighlight"
width: "80%"
height: "80%"
margin: 0.1
minScale: 1
maxScale: 1                
center: false
---

## WHY C#?
<hr>
1. Most of the examples online are written in C#    
2. Same language is used to build Zero Touch Nodes
3. Independent from Dynamo version
4. Faster execution and keyboard shortcuts available

note: 

---

## EXTENDING REVIT
***

1. External command
    - Commands are added to the External Tools pulldown in the ribbon Add-Ins tab

2. External application
    - Applications can create new panels in the ribbon Add-Ins tab
    - External applications can invoke external commands

3. SharpDevelop macro  

##### [source: ADN Revit Training Material, 1_Revit_API_Intro](https://github.com/ADN-DevTech/RevitTrainingMaterial/blob/master/Presentation/1_Revit_API_Intro.pptx)

---

## TOOLS
***
- *RevitLookup*  
Allows you to “snoop” into the Revit database structure. “must have” for any Revit API programmers. Available on ADN DevTech on Github

- *Add-In Manager*  
Allows you to load your dll while running Revit without registering an addin and to rebuild dll without restarting Revit

##### [source: ADN Revit Training Material, 1_Revit_API_Intro](https://github.com/ADN-DevTech/RevitTrainingMaterial/blob/master/Presentation/1_Revit_API_Intro.pptx)
---

## REVIT API DOCUMENTATION
***
[revitapidocs](http://www.revitapidocs.com/) by Gui Talarico

[apidocs.co](https://apidocs.co/) by Gui Talarico

[Autodesk Developer Network](https://www.autodesk.com/developer-network/platform-technologies/revit)

[Revit Training Material](https://github.com/ADN-DevTech/RevitTrainingMaterial )

---

## MACRO MANAGER
<hr>
*Application:*

Macro modules available to all opened Revit projects in the current instance of the Revit application.

<br>
*Active document tab:*

The active document tab represents the currently active project in Revit. The project does not necessarily contain embedded macros.

---

## SHARP DEVELOP
***
Create a module and then as many macro as you need in that module.

---

## HELLO WORLD!
***
```csharp
public void disallowBeamJoins(){
UIDocument uidoc = this.ActiveUIDocument;
Document doc = uidoc.Document;

ICollection<ElementId> selElementsIds = uidoc.Selection
										.GetElementIds();
List<Element> selElements = new List<Element>();
string selected = "";
foreach (var element in selElementsIds) {
	selected += element.ToString() +"\n";
	selElements.Add(doc.GetElement(element));	
}
TaskDialog.Show("Selected Element Id", selected)
}
```
---
```csharp
using (Transaction t = new Transaction 
							(doc, "Disallow Joins")){
	t.Start();
		foreach (Element e in selElements){
			FamilyInstance fa = e as FamilyInstance;
			StructuralFramingUtils.DisallowJoinAtEnd(fa, 0);
			StructuralFramingUtils.DisallowJoinAtEnd(fa, 1);
		}
	t.Commit();
}
TaskDialog.Show("title", selElements.Count.ToString());
}
```

---

## PUBLIC
---
<br>
Available to all callers with access to the type

--

## VOID
---
<br>
The method does not return anything. For example:
```csharp
void Ok_btnClick(object sender, EventArgs e)
{
usertext = textBox1.Text;
}
```
This method sets the value of a variable.

--

## RETURN
---
<br>
This method selects all the View template in the project and
return them as a list.
```csharp
public static List<View> collectTemplates(Document doc)
{
IEnumerable<View> fec = new FilteredElementCollector(doc)
				.OfClass(typeof(View))
				.Cast<View>();

List<View> myVT = new List<View>();
foreach (View v in fec)
{
	if (v.IsTemplate) {
		myVT.Add(v);  }
}
return myVT;
}
```

--

## STATIC
---
<br>
No instance is required to be invoked.

```csharp
List<View> viewTemplates = collectTemplates(doc);
```
<br>
An instance can be created using the *new* keyword:

```csharp
FilteredElementCollector viewTypes = new FilteredElementCollector(doc)
	.OfClass(typeof(ViewFamilyType));
```

--

## WHY I NEED TO CREATE AN INSTANCE OF SOME CLASSES?
---
<br>





## PYTHON to C# #
---
<br>
1. When you declare a variable or constant, you must either specify its type or use the *var* keyword
2. You must end each statement with a semicolon;
3. Double quotes encode a string of multiple characters, single quotes encode a single character (data type *char*)

---

## NAMESPACE
---
<br>
*Python*
```python
import clr
clr.AddReference('RevitAPI')
clr.AddReference('RevitAPIUI')
from Autodesk.Revit.DB import *
from Autodesk.Revit.UI import *
```

*C#*
```csharp
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
```

--

## DOCUMENT MANAGER
---
<br>
*Python*

```python
doc = DocumentManager.Instance.CurrentDBDocument
uidoc = DocumentManager.Instance.CurrentUIApplication.ActiveUIDocument
```

*C#*
```csharp
//Access the UI of the currently Revit project opened
UIDocument uidoc = this.ActiveUIDocument;
//The active or top most view of the project
Document doc = uidoc.Document;
```

--

## SELECTION
---
<br>
*Python*

```python
viewTypes = list(FilteredElementCollector(doc).OfClass(ViewFamilyType))
```

*C#*
```csharp
FilteredElementCollector viewTypes = new FilteredElementCollector(doc)
.OfClass(typeof(ViewFamilyType));
```

--

## FILTER
---
<br>
*Python*

```python
for vt in viewTypes:
if str(vt.ViewFamily) == 'Drafting':
viewType = vt
break
```

*C#*
```csharp
ViewFamilyType vft = null;
foreach (ViewFamilyType vt in viewTypes) {
if (vt.FamilyName == "Drafting View"){
vft = vt;
}
}
```

--

## TRANSACTION
---
<br>
*Python*

```python
t = Transaction (doc, 'Make new Drafting view')
t.Start()
t.Commit()
```

*C#*
```csharp
using (Transaction t = new Transaction(doc))
{
t.Start("Make new Drafting view");
t.Commit();
}
```

--

## CALLING A METHOD
---
<br>
*Python*

```python
newDraftingView = ViewDrafting.Create(doc, viewType.Id)
newDraftingView.Name = textBox.Text
```

*C#*
```csharp
ViewDrafting newDraftingView=ViewDrafting.Create(doc,vft.Id);
newDraftingView.Name = "My New Drafting View";
```
## CODE STRUCTURE
---
<br>
1. Store your methods in a separate Class (i.e. Helpers)
2. These methods must be *public static*
3. Add a Form to the project
4. Create an instance of the Form in ThisApplication
5. Call your methods from ThisApplication (i.e. Helpers.MethodName)

## HELPERS
---
<br>
```csharp
public static List<View> collectTemplates(Document doc){
IEnumerable<View> fec = new FilteredElementCollector(doc)
		.OfClass(typeof(View))
		.Cast<View>();
List<View> myVT = new List<View>();
foreach (View v in fec)
{
if (v.IsTemplate){
myVT.Add(v);
}
}
return myVT;
}
```

--

```csharp
public static
```

```csharp
IEnumerable<View>
```

---

## FORM
---
<br>
```csharp
public partial class Form2 : frms.Form {
public int chosenView;
public Form2(Document doc) {
InitializeComponent();
List<View> viewTemplates = Helpers.collectTemplates(doc);
foreach (var v in viewTemplates) {
comboBoxDrop.Items.Add(v.Name);
}
}
void Form2Load(object sender, EventArgs e){ }
void ComboBox1SelectedIndexChanged(object sender, EventArgs e){
chosenView = comboBoxDrop.SelectedIndex;}
}
```

--

## FORM NAMESPACE
---
<br>
```csharp
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Windows.Forms;
```
When you add the Revit namespace you'll get an error
```csharp
'Form' is an ambiguous reference between 'Autodesk.Revit.DB.Form' and
'System.Windows.Forms.Form' (CS0104)
```
You can solve the ambiguity:
```csharp
using winForms = System.Windows.Forms;
...
public partial class Form1 : winForms.Form
```

--

## Combobox Selected Index Changed Event
---
<br>
```csharp
void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
{
chosenViewTemplate = comboBox1.SelectedIndex;
}
```

--

## Add the document as an argument of the form
---
<br>
```csharp
public CreateDraftingViewForm(Document doc)
```

---

## THIS APPLICATION
---
<br>
```csharp
public void PopulateDropDown()
{
UIDocument uidoc = this.ActiveUIDocument;
Document doc = uidoc.Document;
List<View> allViewTemplates = Helpers
			.collectTemplates(doc);
using(var forma = new Form2(doc)){
//use ShowDialog to show the form as a modal dialog box.
forma.ShowDialog();
TaskDialog.Show("result",
		allViewTemplates[forma.chosenView].Name);
}
```

--

## How to access properties inside classes
---
<br>
```csharp
TaskDialog.Show("ViewTemplateSelected", form.chosenViewTemplate);
```

--

## Use while to keep the Dialog box open
---
<br>
```csharp
string interrupt = "False";
while(interrupt == "False") {
form.ShowDialog();
if (form.usertext.Length >2) {
Helpers.AddDraftingView(doc, form.usertext, form.chosenTemplateId);
interrupt = "True";
}
else if (form.usertext == "") {
TaskDialog.Show("Error", "Please specify the view name"); }
else if (form.usertext.Length <2) {
TaskDialog.Show("Error", "The view name is too short");   }
else { TaskDialog.Show("Error", "I don't know what went wrong");  }
}
```
               
