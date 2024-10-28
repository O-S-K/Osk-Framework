Hi, thank you for using SmartCopier!

Current version: 2.1.5

################
## How to use ##
################

1. Right-click any GameObject or component in the inspector.
2. Click "Smart Copy Components" at the bottom of the context menu.
3. This will open the SmartCopier window where all of the GameObject's Components will be displayed.
4. Toggle any Component and their respective properties you wish to copy.
5. Select any amount of GameObjects you wish to copy the Components to and click "Paste And Replace" or "Paste As New"

#######################################
## Paste And Replace VS Paste As New ##
#######################################

When using "Paste And Replace", SmartCopier will check if the target GameObjects already contain a Component the user wants to copy.
If it does, the Component's property values will simply be replaced by the selected properties.
If the Component does not exist on the target, the Component will be created.

"Paste As New" will always try to add a new Component to the target GameObjects, even if they already have such a component.

There is no difference between "Paste And Replace" and "Paste As New" when selecting only Components that are not present yet in the target GameObjects.

######################
## NoCopy attribute ##
######################

You can add a [NoCopy] attribute above any property you do never wish to copy, like so:

[NoCopy]
public int Number;

This variable will not show up in the list of properties of the SmartCopier window, and its value will not be copied.

######################
## Code-first usage ##
######################

It is entirely possible to use SmartCopier with the use of any UI.
Simply create an instance of the CopyContext class. Your code could look something like this:

var copyContext = new CopyContext(sourceGameObject);
copyContext.PasteComponents(targetGameObject, CopyMode.ReplaceValues);

You can access the ComponentProvider and Components list of the CopyContext to customize its behavior to your liking.
For example, to disable copying a certain type of Component by default, call AddFilteredComponentType<COMPONENTTYPE>() on the ComponentProvider.
The code below will not copy the source GameObject's Transform Component to the target GameObject.

copyContext.ComponentProvider.AddFilteredComponentType<Transform>();
copyContext.RefreshComponents(); // Update the list of Components after adding new filters.
copyContext.PasteComponents(targetGameObject, CopyMode.ReplaceValues);
