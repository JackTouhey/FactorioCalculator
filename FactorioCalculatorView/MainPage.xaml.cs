namespace FactorioCalculatorView;
using System.Collections.ObjectModel;

public partial class MainPage : ContentPage
{
	private List<HorizontalStackLayout> InputRows = new();
	private List<HorizontalStackLayout> OutputRows = new();
	private Dictionary<HorizontalStackLayout, int> ItemQuantities = new();
	private Dictionary<Entry, HorizontalStackLayout> ItemRowFromIPSEntry = new();
	private int CraftSpeed = 1;
	public MainPage()
	{
		InitializeComponent();
	}

	private void InputItemSelected(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e){
		if(InputComboBox.SelectedValue != null){
			String itemName = InputComboBox.SelectedValue.ToString();
			Label SelectedItem = new Label{VerticalTextAlignment = TextAlignment.Center};
			HorizontalStackLayout AddedItemLayout = new HorizontalStackLayout{Spacing = 10.0};
			Entry NumberOfItem = new Entry{Placeholder = "Quantity"};
			String imagePath = @"C:\Users\touheyjack\Documents\VisualStudio\FactorioCalculator\Item Icons\";
			imagePath = imagePath + itemName + ".png";
			Image icon = new Image {Source = imagePath};
			SelectedItem.Text = itemName;
			AddedItemLayout.Add(NumberOfItem);
			AddedItemLayout.Add(icon);
			AddedItemLayout.Add(SelectedItem);
			InputRows.Add(AddedItemLayout);
			InputItems.Add(AddedItemLayout);
		}
	}
	private void OutputItemSelected(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e){
		if(OutputComboBox.SelectedValue != null){
			String itemName = OutputComboBox.SelectedValue.ToString();
			Label SelectedItem = new Label{VerticalTextAlignment = TextAlignment.Center};
			HorizontalStackLayout AddedItemLayout = new HorizontalStackLayout{Spacing = 10.0};
			Entry NumberOfItem = new Entry{Placeholder = "Quantity"};
			String imagePath = @"C:\Users\touheyjack\Documents\VisualStudio\FactorioCalculator\Item Icons\";
			imagePath = imagePath + itemName + ".png";
			Image icon = new Image {Source = imagePath};
			SelectedItem.Text = itemName;
			AddedItemLayout.Add(NumberOfItem);
			AddedItemLayout.Add(icon);
			AddedItemLayout.Add(SelectedItem);
			OutputRows.Add(AddedItemLayout);
			OutputItems.Add(AddedItemLayout);
		}
	}
	private List<Entry> getInputEntries(){
		List<Entry> InputEntries = new();
		foreach(HorizontalStackLayout row in InputRows){
			foreach(Element e in row.Children){
				if(e.GetType() == typeof(Entry)){
					InputEntries.Add((Entry)e);
				}
			}
		}
		return InputEntries;
	}
	private List<Entry> getOutputEntries(){
		List<Entry> OutputEntries = new();
		foreach(HorizontalStackLayout row in OutputRows){
			foreach(Element e in row.Children){
				if(e.GetType() == typeof(Entry)){
					OutputEntries.Add((Entry)e);
				}
			}
		}
		return OutputEntries;
	}
	private void ResetButton(object sender, EventArgs args){
		InputItems.Children.Clear();
		OutputItems.Children.Clear();
		InputItems.Children.Add(InputComboBox);
		OutputItems.Children.Add(OutputComboBox);
		Boolean CompleteRecipePresent = false;
		if(Root.Children.Contains(CompleteRecipe)){
			CompleteRecipePresent = true;
		}
		if(CompleteRecipePresent == false){
			Root.Children.Add(CompleteRecipe);
		}
	}
	private void CompleteRecipeClicked(object sender, EventArgs args){
		Boolean allEntered = true;
		if(CraftSpeedInput.Text == ""){
			allEntered = false;
		}
		foreach(Entry e in getInputEntries()){
			if(e != null){
				if(e.Text == ""){
					allEntered = false;
				}
			}
		}
		foreach(Entry e in getOutputEntries()){
			if(e != null){
				if(e.Text == ""){
					allEntered = false;
				}
			}
		}
		if(allEntered){
			FinalizeRecipe();
		}
		else{
			DisplayAlert("Alert", "Please Fill all text boxes to proceed", "OK");
		}
	}

	private void FinalizeRecipe(){
		try{
			CraftSpeed = int.Parse(CraftSpeedInput.Text);
		}
		catch{
			DisplayAlert("Invalid Craft Speed", "Please use an Integer", "OK");
		}
		RemoveQuantityEntries();
		AddVariableEntries();
	}
	private void AddVariableEntries(){
		foreach(HorizontalStackLayout row in InputRows){
			Entry VariableEntry = new Entry{Placeholder="Enter Items/s"};
			VariableEntry.Completed += OnVariableEntryCompleted;
			row.Add(VariableEntry);
			ItemRowFromIPSEntry.Add(VariableEntry, row);
		}
		foreach(HorizontalStackLayout row in OutputRows){
			Entry VariableEntry = new Entry{Placeholder="Enter Items/s"};
			VariableEntry.Completed += OnVariableEntryCompleted;
			row.Add(VariableEntry);
			ItemRowFromIPSEntry.Add(VariableEntry, row);
		}
	}

    private void OnVariableEntryCompleted(object? sender, EventArgs eArgs)
    {
		Entry SendingEntry = (Entry)sender;
		int VariableItemsPerSecond = 0;
        try{
			VariableItemsPerSecond = int.Parse(SendingEntry.Text);
		}
		catch{
		}
		int VariableQuantity = ItemQuantities[ItemRowFromIPSEntry[SendingEntry]];
		foreach(HorizontalStackLayout row in InputRows){
			Entry EntryToBeRemoved = new();
			Label ItemsPerSecondLabel = new Label{VerticalTextAlignment = TextAlignment.Center};
			foreach(Element e in row.Children){
				if(e.GetType() == typeof(Entry)){
					EntryToBeRemoved = (Entry)e;
				}
			}
			int OwnQuantity = 1;
			if(ItemQuantities[row] != null){
				OwnQuantity = ItemQuantities[row];
			}
			double ItemsPerSecond = (((double)OwnQuantity/VariableQuantity)/CraftSpeed) * VariableItemsPerSecond;
			ItemsPerSecondLabel.Text = $"Items Per Second: {ItemsPerSecond}";
			row.Children.Remove(EntryToBeRemoved);
			row.Children.Add(ItemsPerSecondLabel);
		}
		foreach(HorizontalStackLayout row in OutputRows){
			Entry EntryToBeRemoved = new();
			Label ItemsPerSecondLabel = new Label{VerticalTextAlignment = TextAlignment.Center};
			foreach(Element e in row.Children){
				if(e.GetType() == typeof(Entry)){
					EntryToBeRemoved = (Entry)e;
				}
			}
			int OwnQuantity = 1;
			if(ItemQuantities[row] != null){
				OwnQuantity = ItemQuantities[row];
			}
			double ItemsPerSecond = (((double)OwnQuantity/VariableQuantity)/CraftSpeed) * VariableItemsPerSecond;
			ItemsPerSecondLabel.Text = $"Items Per Second: {ItemsPerSecond}";
			row.Children.Remove(EntryToBeRemoved);
			row.Children.Add(ItemsPerSecondLabel);
		}
    }
	private void RemoveQuantityEntries(){
		foreach(HorizontalStackLayout row in InputRows){
			Label QuantityLabel = new Label{VerticalTextAlignment = TextAlignment.Center};
			Entry EntryToBeRemoved = new();
			foreach(Element e in row.Children){
				if(e.GetType() == typeof(Entry)){
					Entry QuantityEntry = (Entry)e;
					String ItemQuantity = QuantityEntry.Text;
					QuantityLabel.Text = $"{ItemQuantity}";
					EntryToBeRemoved = (Entry)e;
					try{
						int ItemInt = int.Parse(ItemQuantity);
						ItemQuantities.Add(row, ItemInt);
					}
					catch{
						DisplayAlert("Invalid input", "Please only use integers", "OK");
					}
				}
			}
			row.Add(QuantityLabel);
			row.Remove(EntryToBeRemoved);
		}
		foreach(HorizontalStackLayout row in OutputRows){
			Label QuantityLabel = new Label{VerticalTextAlignment = TextAlignment.Center};
			Entry EntryToBeRemoved = new();
			foreach(Element e in row.Children){
				if(e.GetType() == typeof(Entry)){
					Entry QuantityEntry = (Entry)e;
					String ItemQuantity = QuantityEntry.Text;
					QuantityLabel.Text = $"{ItemQuantity}";
					EntryToBeRemoved = (Entry)e;
					try{
						int ItemInt = int.Parse(ItemQuantity);
						ItemQuantities.Add(row, ItemInt);
					}
					catch{
						DisplayAlert("Invalid input", "Please only use integers", "OK");
					}
				}
			}
			row.Add(QuantityLabel);
			row.Remove(EntryToBeRemoved);
		}
		Root.Children.Remove(CompleteRecipe);
	}
	
}

public class Item
{
    public string? Name { get; set; }
	
    public Item(String name){
        this.Name = name;
    }
}

public class ItemViewModels
{
    public ObservableCollection<Item> ItemList { get; set; }
    public ItemViewModels()
    {
        this.ItemList = new ObservableCollection<Item>();
        this.ItemList.Add(new Item("Wooden_chest"));
		this.ItemList.Add(new Item("Iron_chest"));
		this.ItemList.Add(new Item("Steel_chest"));
		this.ItemList.Add(new Item("Storage_tank"));
		this.ItemList.Add(new Item("Transport_belt"));
		this.ItemList.Add(new Item("Fast_transport_belt"));
		this.ItemList.Add(new Item("Express_transport_belt"));
		this.ItemList.Add(new Item("Turbo_transport_belt"));
		this.ItemList.Add(new Item("Underground_belt"));
		this.ItemList.Add(new Item("Fast_underground_belt"));
		this.ItemList.Add(new Item("Express_underground_belt"));
		this.ItemList.Add(new Item("Turbo_underground_belt"));
		this.ItemList.Add(new Item("Splitter"));
		this.ItemList.Add(new Item("Fast_splitter"));
		this.ItemList.Add(new Item("Express_splitter"));
		this.ItemList.Add(new Item("Turbo_splitter"));
		this.ItemList.Add(new Item("Burner_inserter"));
		this.ItemList.Add(new Item("Inserter"));
		this.ItemList.Add(new Item("Long-handed_inserter"));
		this.ItemList.Add(new Item("Fast_inserter"));
		this.ItemList.Add(new Item("Bulk_inserter"));
		this.ItemList.Add(new Item("Stack_inserter"));
		this.ItemList.Add(new Item("Small_electric_pole"));
		this.ItemList.Add(new Item("Medium_electric_pole"));
		this.ItemList.Add(new Item("Big_electric_pole"));
		this.ItemList.Add(new Item("Substation"));
		this.ItemList.Add(new Item("Pipe"));
		this.ItemList.Add(new Item("Pipe_to_ground"));
		this.ItemList.Add(new Item("Pump"));
		this.ItemList.Add(new Item("Rail"));
		this.ItemList.Add(new Item("Rail_ramp"));
		this.ItemList.Add(new Item("Rail_support"));
		this.ItemList.Add(new Item("Train_stop"));
		this.ItemList.Add(new Item("Rail_signal"));
		this.ItemList.Add(new Item("Rail_chain_signal"));
		this.ItemList.Add(new Item("Locomotive"));
		this.ItemList.Add(new Item("Cargo_wagon"));
		this.ItemList.Add(new Item("Fluid_wagon"));
		this.ItemList.Add(new Item("Artillery_wagon"));
		this.ItemList.Add(new Item("Car"));
		this.ItemList.Add(new Item("Tank"));
		this.ItemList.Add(new Item("Spidertron"));
		this.ItemList.Add(new Item("Spidertron_remote"));
		this.ItemList.Add(new Item("Logistic_robot"));
		this.ItemList.Add(new Item("Construction_robot"));
		this.ItemList.Add(new Item("Active_provider_chest"));
		this.ItemList.Add(new Item("Passiv_provider_chest"));
		this.ItemList.Add(new Item("Storage_chest"));
		this.ItemList.Add(new Item("Buffer_chest"));
		this.ItemList.Add(new Item("Requester_chest"));
		this.ItemList.Add(new Item("Roboport"));
		this.ItemList.Add(new Item("Lamp"));
		this.ItemList.Add(new Item("Arithmetic_combinator"));
		this.ItemList.Add(new Item("Decider_combinator"));
		this.ItemList.Add(new Item("Selector_combinator"));
		this.ItemList.Add(new Item("Constant_combinator"));
		this.ItemList.Add(new Item("Power_switch"));
		this.ItemList.Add(new Item("Progammable_speaker"));
		this.ItemList.Add(new Item("Display_panel"));
		this.ItemList.Add(new Item("Stone_brick"));
		this.ItemList.Add(new Item("Concrete"));
		this.ItemList.Add(new Item("Hazard_concrete"));
		this.ItemList.Add(new Item("Refined_concrete"));
		this.ItemList.Add(new Item("Refined_hazard_concrete"));
		this.ItemList.Add(new Item("Landfill"));
		this.ItemList.Add(new Item("Artificial_yumako_soil"));
		this.ItemList.Add(new Item("Overgrowth_yumako_soil"));
		this.ItemList.Add(new Item("Artificial_jellynut_soil"));
		this.ItemList.Add(new Item("Overgrowth_jellynut_soil"));
		this.ItemList.Add(new Item("Ice_platform"));
		this.ItemList.Add(new Item("Foundation"));
		this.ItemList.Add(new Item("Cliff_explosives"));
		this.ItemList.Add(new Item("Repair_pack"));
		this.ItemList.Add(new Item("Boiler"));
		this.ItemList.Add(new Item("Steam_engine"));
		this.ItemList.Add(new Item("Solar_panel"));
		this.ItemList.Add(new Item("Accumulator"));
		this.ItemList.Add(new Item("Nuclear_reactor"));
		this.ItemList.Add(new Item("Heat_pipe"));
		this.ItemList.Add(new Item("Heat_exchanger"));
		this.ItemList.Add(new Item("Steam_turbine"));
		this.ItemList.Add(new Item("Fusion_reactor"));
		this.ItemList.Add(new Item("Fusion_generator"));
		this.ItemList.Add(new Item("Burner_mining_drill"));
		this.ItemList.Add(new Item("Electric_mining_drill"));
		this.ItemList.Add(new Item("Big_mining_drill"));
		this.ItemList.Add(new Item("Offshore_pump"));
		this.ItemList.Add(new Item("Pumpjack"));
		this.ItemList.Add(new Item("Stone_furnace"));
		this.ItemList.Add(new Item("Steel_furnace"));
		this.ItemList.Add(new Item("Electric_furnace"));
		this.ItemList.Add(new Item("Foundry"));
		this.ItemList.Add(new Item("Recycler"));
		this.ItemList.Add(new Item("Agricultural_tower"));
		this.ItemList.Add(new Item("Biochamber"));
		this.ItemList.Add(new Item("Captive_biter_spawner"));
		this.ItemList.Add(new Item("Assembling_machine_1"));
		this.ItemList.Add(new Item("Assembling_machine_2"));
		this.ItemList.Add(new Item("Assembling_machine_3"));
		this.ItemList.Add(new Item("Oil_refinery"));
		this.ItemList.Add(new Item("Chemical_plant"));
		this.ItemList.Add(new Item("Centrifuge"));
		this.ItemList.Add(new Item("Electromagnetic_plant"));
		this.ItemList.Add(new Item("Cryogenic_plant"));
		this.ItemList.Add(new Item("Lab"));
		this.ItemList.Add(new Item("Biolab"));
		this.ItemList.Add(new Item("Lightning_rod"));
		this.ItemList.Add(new Item("Lightning_collector"));
		this.ItemList.Add(new Item("Heating_tower"));
		this.ItemList.Add(new Item("Beacon"));
		this.ItemList.Add(new Item("Speed_module"));
		this.ItemList.Add(new Item("Speed_module_2"));
		this.ItemList.Add(new Item("Speed_module_3"));
		this.ItemList.Add(new Item("Efficiency_module"));
		this.ItemList.Add(new Item("Efficiency_module_2"));
		this.ItemList.Add(new Item("Efficiency_module_3"));
		this.ItemList.Add(new Item("Productivity_module"));
		this.ItemList.Add(new Item("Productivity_module_2"));
		this.ItemList.Add(new Item("Productivity_module_3"));
		this.ItemList.Add(new Item("Quality_module"));
		this.ItemList.Add(new Item("Quality_module_2"));
		this.ItemList.Add(new Item("Quality_module_3"));
		this.ItemList.Add(new Item("Water"));
		this.ItemList.Add(new Item("Steam"));
		this.ItemList.Add(new Item("Crude_oil"));
		this.ItemList.Add(new Item("Heavy_oil"));
		this.ItemList.Add(new Item("Light_oil"));
		this.ItemList.Add(new Item("Lubricant"));
		this.ItemList.Add(new Item("Petroleum_gas"));
		this.ItemList.Add(new Item("Sulfuric_acid"));
		this.ItemList.Add(new Item("Thruster_fuel"));
		this.ItemList.Add(new Item("Thruster_oxidizer"));
		this.ItemList.Add(new Item("Lava"));
		this.ItemList.Add(new Item("Molten_iron"));
		this.ItemList.Add(new Item("Molten_copper"));
		this.ItemList.Add(new Item("Holmium_solution"));
		this.ItemList.Add(new Item("Electrolyte"));
		this.ItemList.Add(new Item("Ammoniacal_solution"));
		this.ItemList.Add(new Item("Ammonia"));
		this.ItemList.Add(new Item("Fluorine"));
		this.ItemList.Add(new Item("Fluoroketone_(hot)"));
		this.ItemList.Add(new Item("Fluoroketone_(cold)"));
		this.ItemList.Add(new Item("Lithium_brine"));
		this.ItemList.Add(new Item("Plasma"));
		this.ItemList.Add(new Item("Wood"));
		this.ItemList.Add(new Item("Coal"));
		this.ItemList.Add(new Item("Stone"));
		this.ItemList.Add(new Item("Iron_ore"));
		this.ItemList.Add(new Item("Iron_plate"));
		this.ItemList.Add(new Item("Copper_ore"));
		this.ItemList.Add(new Item("Uranium_ore"));
		this.ItemList.Add(new Item("Raw_fish"));
		this.ItemList.Add(new Item("Ice"));
		this.ItemList.Add(new Item("Water_barrel"));
		this.ItemList.Add(new Item("Crude_oil_barrel"));
		this.ItemList.Add(new Item("Petroleum_gas_barrel"));
		this.ItemList.Add(new Item("Light_oil_barrel"));
		this.ItemList.Add(new Item("Heavy_oil_barrel"));
		this.ItemList.Add(new Item("Lubricant_barrel"));
		this.ItemList.Add(new Item("Sulfuric_acid_barrel"));
		this.ItemList.Add(new Item("Fluoroketone_(hot)_barrel"));
		this.ItemList.Add(new Item("Fluoroketone_(cold)_barrel"));
		this.ItemList.Add(new Item("Iron_gear_wheel"));
		this.ItemList.Add(new Item("Iron_stick"));
		this.ItemList.Add(new Item("Copper_cable"));
		this.ItemList.Add(new Item("Barrel"));
		this.ItemList.Add(new Item("Electronic_circuit"));
		this.ItemList.Add(new Item("Advanced_circuit"));
		this.ItemList.Add(new Item("Processing_unit"));
		this.ItemList.Add(new Item("Engine_unit"));
		this.ItemList.Add(new Item("Electric_engine_unit"));
		this.ItemList.Add(new Item("Flying_robot_frame"));
		this.ItemList.Add(new Item("Low_density_structure"));
		this.ItemList.Add(new Item("Rocket_fuel"));
		this.ItemList.Add(new Item("Rocket_part"));
		this.ItemList.Add(new Item("Uranium-235"));
		this.ItemList.Add(new Item("Uranium-238"));
		this.ItemList.Add(new Item("Uranium_fuel_cell"));
		this.ItemList.Add(new Item("Depleted_uranium_fuel_cell"));
		this.ItemList.Add(new Item("Nuclear_fuel"));
		this.ItemList.Add(new Item("Calcite"));
		this.ItemList.Add(new Item("Tungsten_ore"));
		this.ItemList.Add(new Item("Tungsten_carbide"));
		this.ItemList.Add(new Item("Tungsten_plate"));
		this.ItemList.Add(new Item("Scrap"));
		this.ItemList.Add(new Item("Holmium_ore"));
		this.ItemList.Add(new Item("Holmium_plate"));
		this.ItemList.Add(new Item("Superconductor"));
		this.ItemList.Add(new Item("Supercapacitor"));
		this.ItemList.Add(new Item("Yumako_seed"));
		this.ItemList.Add(new Item("Jellynut_seed"));
		this.ItemList.Add(new Item("Tree_seed"));
		this.ItemList.Add(new Item("Yumako"));
		this.ItemList.Add(new Item("Jellynut"));
		this.ItemList.Add(new Item("Iron_bacteria"));
		this.ItemList.Add(new Item("Copper_bacteria"));
		this.ItemList.Add(new Item("Spoilage"));
		this.ItemList.Add(new Item("Nutrients"));
		this.ItemList.Add(new Item("Bioflux"));
		this.ItemList.Add(new Item("Yumako_mash"));
		this.ItemList.Add(new Item("Jelly"));
		this.ItemList.Add(new Item("Carbon_fiber"));
		this.ItemList.Add(new Item("Biter_egg"));
		this.ItemList.Add(new Item("Pentapod_egg"));
		this.ItemList.Add(new Item("Lithium"));
		this.ItemList.Add(new Item("Lithium_plate"));
		this.ItemList.Add(new Item("Quantum_processor"));
		this.ItemList.Add(new Item("Automation_science_pack"));
		this.ItemList.Add(new Item("Logistic_science_pack"));
		this.ItemList.Add(new Item("Military_science_pack"));
		this.ItemList.Add(new Item("Chemical_science_pack"));
		this.ItemList.Add(new Item("Production_science_pack"));
		this.ItemList.Add(new Item("Utility_science_pack"));
		this.ItemList.Add(new Item("Space_science_pack"));
		this.ItemList.Add(new Item("Metallurgic_science_pack"));
		this.ItemList.Add(new Item("Electromagnetic_science_pack"));
		this.ItemList.Add(new Item("Agricultural_science_pack"));
		this.ItemList.Add(new Item("Cryogenic_science_pack"));
		this.ItemList.Add(new Item("Promethium_science_pack"));
		this.ItemList.Add(new Item("Rocket_silo"));
		this.ItemList.Add(new Item("Cargo_landing_pad"));
		this.ItemList.Add(new Item("Space_platform_foundation"));
		this.ItemList.Add(new Item("Cargo_bay"));
		this.ItemList.Add(new Item("Asteroid_collector"));
		this.ItemList.Add(new Item("Crusher"));
		this.ItemList.Add(new Item("Thruster"));
		this.ItemList.Add(new Item("Space_platform_hub"));
		this.ItemList.Add(new Item("Satellite"));
		this.ItemList.Add(new Item("Space_platform_starter_pack"));
		this.ItemList.Add(new Item("Metallic_asteroid_chunk"));
		this.ItemList.Add(new Item("Carbonic_asteroid_chunk"));
		this.ItemList.Add(new Item("Oxide_asteroid_chunk"));
		this.ItemList.Add(new Item("Promethium_asteroid_chunk"));
		this.ItemList.Add(new Item("Pistol"));
		this.ItemList.Add(new Item("Submachine_gun"));
		this.ItemList.Add(new Item("Railgun"));
		this.ItemList.Add(new Item("Tesla_gun"));
		this.ItemList.Add(new Item("Shotgun"));
		this.ItemList.Add(new Item("Combat_shotgun"));
		this.ItemList.Add(new Item("Rocket_launcher"));
		this.ItemList.Add(new Item("Flamethrower"));
		this.ItemList.Add(new Item("Firearm_magazine"));
		this.ItemList.Add(new Item("Piercing_rounds_magazine"));
		this.ItemList.Add(new Item("Uranium_rounds_magazine"));
		this.ItemList.Add(new Item("Shotgun_shells"));
		this.ItemList.Add(new Item("Piercing_shotgun_shells"));
		this.ItemList.Add(new Item("Cannon_shell"));
		this.ItemList.Add(new Item("Explosive_cannon_shell"));
		this.ItemList.Add(new Item("Uranium_cannon_shell"));
		this.ItemList.Add(new Item("Explosive_uranium_cannon_shell"));
		this.ItemList.Add(new Item("Artillery_shell"));
		this.ItemList.Add(new Item("Rocket"));
		this.ItemList.Add(new Item("Explosive_rocket"));
		this.ItemList.Add(new Item("Atomic_bomb"));
		this.ItemList.Add(new Item("Capture_bot_rocket"));
		this.ItemList.Add(new Item("Flamethrower_ammo"));
		this.ItemList.Add(new Item("Railgun_ammo"));
		this.ItemList.Add(new Item("Tesla_ammo"));
		this.ItemList.Add(new Item("Grenade"));
		this.ItemList.Add(new Item("Cluster_grenade"));
		this.ItemList.Add(new Item("Poison_capsule"));
		this.ItemList.Add(new Item("Slowdown_capsule"));
		this.ItemList.Add(new Item("Defender_capsule"));
		this.ItemList.Add(new Item("Distractor_capsule"));
		this.ItemList.Add(new Item("Destroyer_capsule"));
		this.ItemList.Add(new Item("Light_armor"));
		this.ItemList.Add(new Item("Heavy_armor"));
		this.ItemList.Add(new Item("Modular_armor"));
		this.ItemList.Add(new Item("Power_armor"));
		this.ItemList.Add(new Item("Power_armor_MK2"));
		this.ItemList.Add(new Item("Mech_armor"));
		this.ItemList.Add(new Item("Portable_solar_panel"));
		this.ItemList.Add(new Item("Portable_fission_reactor"));
		this.ItemList.Add(new Item("Portable_fusion_reactor"));
		this.ItemList.Add(new Item("Personal_battery"));
		this.ItemList.Add(new Item("Personal_battery_MK2"));
		this.ItemList.Add(new Item("Personal_battery_MK3"));
		this.ItemList.Add(new Item("Belt_immunity_equipment"));
		this.ItemList.Add(new Item("Exoskeleton"));
		this.ItemList.Add(new Item("Personal_roboport"));
		this.ItemList.Add(new Item("Personal_roboport_MK2"));
		this.ItemList.Add(new Item("Nightvision"));
		this.ItemList.Add(new Item("Toolbelt_equipment"));
		this.ItemList.Add(new Item("Energy_shield"));
		this.ItemList.Add(new Item("Energy_shield_MK2"));
		this.ItemList.Add(new Item("Personal_laser_defense"));
		this.ItemList.Add(new Item("Discharge_defense"));
		this.ItemList.Add(new Item("Discharge_defense_remote"));
		this.ItemList.Add(new Item("Wall"));
		this.ItemList.Add(new Item("Gate"));
		this.ItemList.Add(new Item("Radar"));
		this.ItemList.Add(new Item("Land_mine"));
		this.ItemList.Add(new Item("Gun_turret"));
		this.ItemList.Add(new Item("Laser_turret"));
		this.ItemList.Add(new Item("Flamethrower_turret"));
		this.ItemList.Add(new Item("Artillery_turret"));
		this.ItemList.Add(new Item("Rocket_turret"));
		this.ItemList.Add(new Item("Tesla_turret"));
		this.ItemList.Add(new Item("Railgun_turret"));
    }
}


