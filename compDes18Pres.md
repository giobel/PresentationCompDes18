---
customTheme : "myTheme"
transition: "none"
#highlightTheme: "revitHighlight"
slideNumber: true
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
## DISCLAIMER!
***
1. This is not a class on C# language
2. There are many C# fundamentals concept that will not be covered
3. You will leave with lots of homework to do
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

## LET'S CREATE A MACRO
***

<img src="images/macro001.PNG" width="900">

---
    [Autodesk.Revit.DB.Macros.AddInId("C329BC57-C708-4670-9239-A40E1CED1CE0")]
	public partial class ThisApplication{
		public void MyFirstMacro()
    	{
    		TaskDialog.Show("Dialog Title", "My first Macro!");
   		}
	}
}
```

---

- Namespace
- Class
- Method

---
## LIST, COLLECTION, ILIST, ICOLLECTION

---
## ACCESS MODIFIERS
***
- **PUBLIC** Available to all callers with access to the type
- **PROTECTED**
- **INTERNAL**
- **PRIVATE**

<img src="images/modifiers.png" width="600">

[docs.microsoft.com](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/access-modifiers)

---

## METHODS
***
- **VOID** The method does not return anything. For example:
```csharp
void Ok_btnClick(object sender, EventArgs e)
{
usertext = textBox1.Text;
}
```
This method sets the value of a variable.

---

## RETURN
***
This method selects all the View template in the project and
return them as a list.
```csharp
public static List<View> collectTemplates(Document doc)
{
IEnumerable<View> fec = new FilteredElementCollector(doc).OfClass(typeof(View)).Cast<View>();

List<View> myVT = new List<View>();

foreach (View v in fec)
{
	if (v.IsTemplate) 
		{
		myVT.Add(v);  
		}
}
return myVT;
}
```

---
## CAST
***

---

## STATIC
***
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

---

## WHY I NEED TO CREATE AN INSTANCE OF SOME CLASSES?
***

---

## CODE STRUCTURE

1. Store your methods in a separate Class (i.e. Helpers)
2. These methods must be *public static*
3. Add a Form to the project
4. Create an instance of the Form in ThisApplication
5. Call your methods from ThisApplication (i.e. Helpers.MethodName)

---

## HELPERS
***
```csharp
public static void deleteElements(UIDocument uidoc, Type typeToDelete)
	{
	Document doc = uidoc.Document;
	IList<Element> sheets = new FilteredElementCollector(doc).OfClass(typeToDelete).ToElements();
	string s = "";
		using (Transaction t = new Transaction (doc, "Delete Sheets")){
		t.Start();
		foreach (Element e in sheets) {
			ElementId eid = e.Id;
			try{
				doc.Delete(eid);
				s += eid.ToString() + " deleted" + Environment.NewLine;
			}
			catch{
				s += eid.ToString() + " failed" + "/n";
			}
		}
		t.Commit();
	}
	TaskDialog.Show("Delete Sheets", s);
}//close method
```
---
## THIS APPLICATION
***
```csharp
public void deleteElementsForm(){
	UIDocument uidoc = this.ActiveUIDocument;
	Helpers.deleteElements(uidoc, typeof(ViewSchedule)); 
	//error if not static -> object reference is required
}
```
---
## CREATE FORM
***
<img src="images/macro002.PNG" width="900">

---
## FORM METHODS
***

```csharp
public partial class Form2 : winForm.Form
{
	public Form2()
	{
		InitializeComponent();
	}
			
	public bool schedulesSelected; 
	public bool sheetsSelected;

	
	void DeleteBtnClick(object sender, System.EventArgs e)
	{	
		schedulesSelected = checkBoxSchedules.Checked;
		sheetsSelected = checkBoxSheets.Checked;	
	}
}
```

---
## LOAD FORM
***
```csharp
 public void deleteElementsForm(){
		   	
   UIDocument uidoc = this.ActiveUIDocument;
   Document doc = uidoc.Document;
   
   using (var form = new Form2()) {
				
		form.ShowDialog();				
		
		if (form.DialogResult == winForms.DialogResult.OK)
		{
			if (form.schedulesSelected == true)
				Helpers.deleteElements(uidoc, typeof(ViewSchedule));
			
			if (form.sheetsSelected == true)
				Helpers.deleteElements(uidoc, typeof(ViewSheet));
		}
		
		}
}//close macro
```
---
## ADD TEXT LABELS
***

```csharp
public partial class Form2 : winForm.Form
{
	public Form2()
	{
		InitializeComponent();
	}
			
	public bool schedulesSelected; 
	public bool sheetsSelected;
	
	void DeleteBtnClick(object sender, System.EventArgs e)
		{	
		schedulesSelected = checkBoxSchedules.Checked;
		sheetsSelected = checkBoxSheets.Checked;	
		}
	
	public int sheetsCount;
	public int schedulesCount;
	
	void CheckBoxSheetsCheckedChanged(object sender, EventArgs e)
	{
		if (checkBoxSheets.Checked)
			labelSheets.Text = sheetsCount.ToString() + " selected";
		else
			labelSheets.Text = "";
	}
	void CheckBoxSchedulesCheckedChanged(object sender, EventArgs e)
	{
		if (checkBoxSchedules.Checked)
			labelSchedules.Text = schedulesCount.ToString() + " selected";
		else
			labelSchedules.Text = "";
	}
}
```
---
## THIS APPLICATION
***
```csharp
 public void deleteElementsForm(){
   UIDocument uidoc = this.ActiveUIDocument;
   Document doc = uidoc.Document;
   using (var form = new Form2()) {
		form.sheetsCount = Helpers.countElements(uidoc,typeof(ViewSheet)); 
		form.schedulesCount = Helpers.countElements(uidoc,typeof(ViewSchedule));
		//Set variable before form.ShowDialog()
		form.ShowDialog();				
		if (form.DialogResult == winForms.DialogResult.OK)
		{
			if (form.schedulesSelected == true)
				Helpers.deleteElements(uidoc, typeof(ViewSchedule));
			
			if (form.sheetsSelected == true)
				Helpers.deleteElements(uidoc, typeof(ViewSheet));
		}
		}
}//close macro
```
---
# APPENDIX

---

## FORM NAMESPACE AMBIGUITY
***
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
---
## CHECHED LIST BOX
***
```csharp
public partial class Form2 : frms.Form
{
    public int chosenView;
    public Form2(Document doc) //Pass the document as argument
    {
        InitializeComponent();
        List<View> viewTemplates = Helpers.collectTemplates(doc);
        foreach (var v in viewTemplates)
        {
            comboBoxDrop.Items.Add(v.Name);
        }
    }
    void Form2Load(object sender, EventArgs e) { }
    void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
    {
        chosenView = comboBoxDrop.SelectedIndex;
    }
}
```

---

## COMBOBOX (DROPDOWN MENU)
***
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
---

## WHILE LOOP
***
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
	else if (form.usertext.Length<2) {
		TaskDialog.Show("Error", "The view name is too short");   }
	else { 
		TaskDialog.Show("Error", "I don't know what went wrong");  }
}
```

---
<iframe src="https://giobel.github.io/PresentationCompDes18/compDes18Pres-export/loader" width= "1200"  height="500" scrolling="no"></iframe>

---
<a href="http://tt-acm.github.io/Spectacles.WebViewer/" data-preview-link>spectacles</a>