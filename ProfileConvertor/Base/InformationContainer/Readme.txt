
* InformationContainer: added to the profile, each object refers to information like snapshots, covers ...etc

* InformationContainerConsoleItem: added to all consoles of the profile for an "InformationContainer" object, have
the same id of that object. Used to set information info for each console of the InformationContainer object, for example,
For an InformationContainer object for snapshots added in profile, a InformationContainerConsoleItem must be created in all
the consoles of that profile, and each InformationContainerConsoleItem must hold the snapshots folder for each console.

* InformationContainerItem: added to a rom for an "InformationContainer" object, includes the accual information for the item.
For example, if we have InformationContainer object for snapshots then the rom will contain the InformationContainerItem for
snapshot file paths. 

WARNING: you must add System.Xml.Serialization.XmlInclude attribut and include the new information container and it's related
items that descripted above to Profile.cs, Console.cs and Rom.cs, this step required for xml serialization so that not fail
on xml export. 