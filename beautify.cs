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
else { TaskDialog.Show("Error", "I don't know what went wrong");  }
}