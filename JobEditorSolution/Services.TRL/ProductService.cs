 
using Communication.Plc.Channel;
using Communication.Plc.Shared;
using Messages;
using Models;
using Patterns.EventAggregator;
using ProductLib;
using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Services.TRL
{
	public class ProductService : ServiceBase<ProductService>, IServiceBaseDerived, IModel
	{
		private string productFileName = string.Empty;

		private List<PlcProductionDataRaw> jobs;

		private List<PlcLayerDataRaw> layers;

		private PlcMachineConfigurationDataRaw machineConfig;

		private PlcServices plcServices;

		private List<ServiceDataObject> dataObjects;

		private List<ServiceDataObject> plcDataObjects;

		private CommandHandle plcHandle;

		public ProductDefinition Product
		{
			get;
			set;
		}

		public ProductLib.Product ProductOrg
		{
			get;
			set;
		}

		public ProductService(UIProtoType uiProtoType)
		{
			this.Init(uiProtoType, typeof(ActivationClient));
			this.dataObjects = new List<ServiceDataObject>();
			this.plcDataObjects = new List<ServiceDataObject>();
			base.RegisterType<PlcProductionClientDataRaw>();
			base.RegisterType<PlcLayerDataRaw>();
			base.RegisterType<PlcProductionDataRaw>();
			base.RegisterType<PlcMachineConfigurationDataRaw>();
		}

		private double ConvertToUnit(double value, bool UnitIsInch)
		{
			if (!UnitIsInch)
			{
				return value;
			}
			return value / 25.4;
		}

		private int Execute(string exe, string arguments)
		{
			int num;
			string directoryName = Path.GetDirectoryName(exe);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(exe);
			Process process = new Process();
			process.StartInfo.FileName = fileNameWithoutExtension;
			process.StartInfo.WorkingDirectory = directoryName;
			process.StartInfo.Arguments = arguments;
			try
			{
				process.Start();
				process.WaitForExit();
				return process.ExitCode;
			}
			catch (Exception exception)
			{
				num = 1;
			}
			return num;
		}

		private PlcProductionClientDataRaw? GetClientData(List<ServiceDataObject> dataObjects)
		{
			PlcProductionClientDataRaw? nullable;
			List<ServiceDataObject>.Enumerator enumerator = dataObjects.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ServiceDataObject current = enumerator.Current;
					if (current.DataPntr.Type != "T_ProductionClientData")
					{
						continue;
					}
					nullable = new PlcProductionClientDataRaw?((PlcProductionClientDataRaw)current.Data);
					return nullable;
				}
				return null;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return nullable;
		}

		private void GetDataObjects(CommandHandle handle)
		{
			// 
			// Current member / type: System.Void Services.TRL.ProductService::GetDataObjects(Models.CommandHandle)
			// File path: D:\El PLant\DOK Job Editor Priprema\v3 dlls\costura.Services.trl.dll
			// 
			// Product version: 2018.2.803.0
			// Exception in: System.Void GetDataObjects(Models.CommandHandle)
			// 
			// Object reference not set to an instance of an object.
			//    at ..( , Int32 , Statement& , Int32& ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Steps\CodePatterns\ObjectInitialisationPattern.cs:line 78
			//    at ..( , Int32& , Statement& , Int32& ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Steps\CodePatterns\BaseInitialisationPattern.cs:line 33
			//    at ..( ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Steps\CodePatternsStep.cs:line 57
			//    at ..(ICodeNode ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 49
			//    at ..Visit(ICodeNode ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 276
			//    at ..( ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 396
			//    at ..(ICodeNode ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 63
			//    at ..Visit(ICodeNode ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 276
			//    at ..Visit[,]( ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 286
			//    at ..Visit( ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 317
			//    at ..( ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 337
			//    at ..( ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Steps\CodePatternsStep.cs:line 39
			//    at ..(ICodeNode ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 49
			//    at ..Visit(ICodeNode ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Ast\BaseCodeTransformer.cs:line 276
			//    at ..(DecompilationContext ,  ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Steps\CodePatternsStep.cs:line 33
			//    at ..(MethodBody ,  , ILanguage ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:line 88
			//    at ..(MethodBody , ILanguage ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:line 70
			//    at Telerik.JustDecompiler.Decompiler.Extensions.( , ILanguage , MethodBody , DecompilationContext& ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs:line 95
			//    at Telerik.JustDecompiler.Decompiler.Extensions.(MethodBody , ILanguage , DecompilationContext& ,  ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs:line 58
			//    at ..(ILanguage , MethodDefinition ,  ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\WriterContextServices\BaseWriterContextService.cs:line 117
			// 
			// mailto: JustDecompilePublicFeedback@telerik.com

		}

		private CommandHandle GetHandle(object obj)
		{
			CommandHandle commandHandle;
			if (!(obj is PlcCommandHandleRaw))
			{
				commandHandle = obj as CommandHandle;
			}
			else
			{
				commandHandle = new CommandHandle();
				commandHandle = commandHandle.Get((PlcCommandHandleRaw)obj);
			}
			return commandHandle;
		}

		private string GetProductFileName(ServiceProductData data)
		{
			return string.Concat(data.PathProduct, data.FileProduct);
		}

		private PlcLayerDataRaw LayerToLayerData(Layer layer, int id, bool absolute)
		{
			PlcLayerDataRaw plcLayerDataRaw = new PlcLayerDataRaw()
			{
				Index = (short)(id + 1),
				ProductLink = -1,
				MeasuredProductId = -1,
				ReferenceEna = true,
				CorrectionEna = (layer.LayerDefault.HeightCorrType == EHeightCorrectionType.None ? false : this.machineConfig.HeightCorrectionEnable),
				CorrectionUp = (layer.LayerDefault.HeightCorrType == EHeightCorrectionType.CompleteCycleUp ? true : layer.LayerDefault.HeightCorrType == EHeightCorrectionType.PreciseUp),
				CorrectionBySheet = (layer.LayerDefault.HeightCorrType == EHeightCorrectionType.PreciseDown ? true : layer.LayerDefault.HeightCorrType == EHeightCorrectionType.PreciseUp),
				Absolute = absolute,
				ReferenceDone = false,
				CalculationDone = false,
				CorrectionDone = false,
				RefMeasurement = 0,
				CorrMeasurement = 0,
				CorrectedTodoProPile = 0
			};
			return plcLayerDataRaw;
		}

		private PlcProductionDataRaw LayerToProductionData(Layer layer, int id, EHeightRefType heightRefType)
		{
			PlcProductionDataRaw unit = new PlcProductionDataRaw()
			{
				IsReload = false,
				Selected = false,
				CurrentState = 3,
				FileIndex = (short)id,
				Index = (short)id,
				Layer = (short)(layer.Id + 1),
				NextOnLayer = -1,
				PrevOnLayer = -1,
				DoneLocked = false,
				TodoLocked = false,
				ReferenceDone = false,
				CalculationDone = false,
				CorrectionDone = false,
				RefMeasurement = 0,
				CorrMeasurement = 0,
				CorrectedTodoProPile = 0,
				MeasureAtSheet = 0,
				Done = 0,
				Thickness = layer.LayerDefault.MaterialThickness,
				Width = layer.LayerDefault.Width,
				ProductName = Path.GetFileNameWithoutExtension(this.productFileName),
				ProductFile = this.productFileName,
				Info = layer.Info
			};
            unit.LayerName = unit.ProductName + layer.Name;
            if (unit.Thickness < this.ConvertToUnit(0.01, this.ProductOrg.UnitsInInches))
			{
				unit.Thickness = this.ConvertToUnit(0.3, this.ProductOrg.UnitsInInches);
			}
			if (heightRefType != EHeightRefType.MM)
			{
				unit.Height = layer.LayerDefault.Height * unit.Thickness;
				unit.ToDo = (ushort)(layer.LayerDefault.Height * (double)layer.Shapes.Count);
			}
			else
			{
				unit.Height = layer.LayerDefault.Height;
				unit.ToDo = (ushort)(layer.LayerDefault.Height / unit.Thickness * (double)layer.Shapes.Count);
			}
			unit.ShapeCount = (short)layer.Shapes.Count;
			unit.ShapeInfo = new PlcProductionShapeInfoDataRaw[5];
			int num = -1;
			foreach (Shape shape in layer.Shapes)
			{
				num++;
				unit.ShapeInfo[num].Highlight = true;
				unit.ShapeInfo[num].ProductsDone = 0;
				unit.ShapeInfo[num].StackNr = (short)num;
			}
			return unit;
		}

		private void Merge(List<PlcProductionDataRaw> currentJobs, List<PlcLayerDataRaw> currentLayers)
		{
			if (currentJobs.Count <= 0 || this.jobs.Count <= 0)
			{
				return;
			}
			foreach (PlcProductionDataRaw currentJob in currentJobs)
			{
				if (currentJob.ProductFile == this.jobs[0].ProductFile)
				{
					continue;
				}
				this.jobs.Add(currentJob);
			}
			for (int i = 0; i < this.jobs.Count; i++)
			{
				PlcProductionDataRaw item = this.jobs[i];
				for (int j = 0; j < currentJobs.Count; j++)
				{
					if (currentJobs[j].ProductFile == item.ProductFile && Math.Abs(currentJobs[j].Width - item.Width) < 0.001 && currentJobs[j].ShapeCount == item.ShapeCount)
					{
						item.IsReload = true;
						item.ToDo = currentJobs[j].ToDo;
						item.Done = currentJobs[j].Done;
						item.Height = item.Thickness * (double)item.ToDo;
						item.CurrentState = currentJobs[j].CurrentState;
						item.ShapeInfo = currentJobs[j].ShapeInfo;
					}
				}
				this.jobs[i] = item;
			}
		}

		private void NotifyEditor(ProductService.Commands cmd)
		{
			Address address = new Address(this.Sender.Owner, "JobEditor.Activate.Start", "", null);
			this.aggregator.Publish<Telegram>(new Telegram(address, (int)cmd, null, null), true);
		}

		private void NotifyPlc(ProductService.Commands cmd)
		{
			if (this.plcServices == null)
			{
				return;
			}
			string name = this.dataObjects[0].telegram.CommAddress.Owner.Name;
			if (this.plcServices.IsPlcAddress(name))
			{
				name = this.plcServices.AddChannel(name);
			}
			Address address = new Address(this.Sender.Owner, name, "", null);
			this.aggregator.Publish<Telegram>(new Telegram(address, (int)cmd, null, null), true);
		}

		public override void OnNotifiedByClient(int command)
		{
			ProductService.Commands command1 = (ProductService.Commands)base.CurrentTelegram.Command;
			if (command1 != ProductService.Commands.ProcessJobs)
			{
				if (command1 != ProductService.Commands.RequestState)
				{
					return;
				}
				this.NotifyPlc(ProductService.Commands.RequestState);
			}
			else if (base.CurrentTelegram.Value is ServiceProductData)
			{
				if (this.plcServices == null || this.plcHandle == null)
				{
					return;
				}
				this.GetDataObjects(this.plcHandle);
				this.plcDataObjects = this.dataObjects;
				ServiceProductData value = (ServiceProductData)base.CurrentTelegram.Value;
				this.productFileName = Path.Combine(value.PathProduct, value.FileProduct);
				if (!this.ProcessProduct(this.productFileName, this.plcDataObjects))
				{
					return;
				}
				List<PlcProductionDataRaw> plcProductionDataRaws = this.ReadJobs(this.plcDataObjects);
				this.Merge(plcProductionDataRaws, this.ReadLayers(this.plcDataObjects));
				this.jobs = this.SortJobs(this.jobs);
				this.WriteJobs(this.plcDataObjects, this.jobs);
				this.WriteLayers(this.plcDataObjects, this.layers);
				this.NotifyPlc(ProductService.Commands.InitProductList);
				return;
			}
		}

		public override void OnNotifiedByPlc(int command)
		{
			PlcProductionClientDataRaw? clientData = null;
			PlcProductionDataRaw? nullable = null;
			this.plcServices = (PlcServices)base.CurrentTelegram.ServiceLink;
			CommandHandle handle = this.GetHandle(base.CurrentTelegram.Value);
			this.plcHandle = handle;
			ProductService.Commands command1 = (ProductService.Commands)handle.Command;
			switch (command1)
			{
				case ProductService.Commands.GenerateAndLoadNc:
				{
					this.GetDataObjects(handle);
					this.plcDataObjects = this.dataObjects;
					clientData = this.GetClientData(this.dataObjects);
					if (!clientData.HasValue)
					{
						return;
					}
					nullable = this.ReadJob(this.dataObjects, clientData.Value.Index);
					if (nullable.HasValue && this.RunTrafoProcess(nullable.Value.ProductFile, nullable.Value.FileIndex, (int)nullable.Value.ToDo, (int)nullable.Value.Done, clientData.Value.ProcessPath))
					{
						this.NotifyPlc(ProductService.Commands.InitNc);
						return;
					}
					this.NotifyPlc(ProductService.Commands.NcFault);
					return;
				}
				case ProductService.Commands.ParseAndLoadProductFile:
				{
					return;
				}
				case ProductService.Commands.Create:
				{
					this.GetDataObjects(handle);
					this.plcDataObjects = this.dataObjects;
					return;
				}
				default:
				{
					if (command1 == ProductService.Commands.NoJobActive)
					{
						this.NotifyEditor(ProductService.Commands.NoJobActive);
						return;
					}
					if (command1 == ProductService.Commands.JobActive)
					{
						this.NotifyEditor(ProductService.Commands.JobActive);
						return;
					}
					if (!this.SelectFile(ref this.productFileName))
					{
						return;
					}
					if (!this.ProcessProduct(this.productFileName, this.plcDataObjects))
					{
						return;
					}
					this.WriteJobs(this.dataObjects, this.jobs);
					this.NotifyPlc(ProductService.Commands.InitProductList);
					return;
				}
			}
		}

		private bool ProcessJobs()
		{
			if (this.Product == null)
			{
				return false;
			}
			if (this.jobs == null)
			{
				this.jobs = new List<PlcProductionDataRaw>();
			}
			this.jobs.Clear();
			int num = 0;
			foreach (Layer layer in this.Product.ProductData.Layers)
			{
				int num1 = num;
				num = num1 + 1;
				this.jobs.Add(this.LayerToProductionData(layer, num1, this.Product.ProductData.HeightRefType));
			}
			return true;
		}

		private bool ProcessLayers()
		{
			if (this.ProductOrg == null)
			{
				return false;
			}
			if (this.layers == null)
			{
				this.layers = new List<PlcLayerDataRaw>();
			}
			this.layers.Clear();
			int num = 0;
			foreach (Layer layer in this.ProductOrg.Layers)
			{
				int num1 = num;
				num = num1 + 1;
				this.layers.Add(this.LayerToLayerData(layer, num1, this.ProductOrg.HeightMeasType == EHeightMeasType.Absolute));
			}
			return true;
		}

		private bool ProcessMachine(List<ServiceDataObject> plcDataObjects)
		{
			this.ReadMachineConfig(plcDataObjects);
			return true;
		}

		private void ProcessPileCorrection()
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				PlcProductionDataRaw item = this.jobs[i];
				if (item.FileIndex < this.Product.ProductData.Layers.Count)
				{
					PlcLayerDataRaw plcLayerDataRaw = this.layers[item.Layer - 1];
					if (plcLayerDataRaw.CorrectionEna)
					{
						Layer layer = this.Product.ProductData.Layers[item.FileIndex];
						int numberOfSteps = layer.LayerDefault.NumberOfSteps * layer.LayerDefault.NumberOfSame;
						int toDo = item.ToDo / item.ShapeInfo.Count<PlcProductionShapeInfoDataRaw>();
						int num = toDo / numberOfSteps;
						if (toDo >= 2)
						{
							short num1 = (short)((double)toDo * 0.8);
							num1 = (short)(num1 * item.ShapeInfo.Count<PlcProductionShapeInfoDataRaw>());
							item.MeasureAtSheet = num1;
							this.jobs[i] = item;
						}
						else
						{
							plcLayerDataRaw.CorrectionEna = false;
						}
					}
				}
			}
			double height = 0;
			for (int j = 0; j < this.layers.Count; j++)
			{
				PlcLayerDataRaw item1 = this.layers[j];
				item1.HeightTrgPrev = height;
				height += item1.Height;
				this.layers[j] = item1;
			}
			for (int k = 0; k < this.jobs.Count; k++)
			{
				PlcProductionDataRaw heightTrgPrev = this.jobs[k];
				PlcLayerDataRaw plcLayerDataRaw1 = this.layers[heightTrgPrev.Layer - 1];
				if (plcLayerDataRaw1.Absolute)
				{
					heightTrgPrev.Height += plcLayerDataRaw1.HeightTrgPrev;
					this.jobs[k] = heightTrgPrev;
				}
			}
		}

		private bool ProcessProduct(string productFileName, List<ServiceDataObject> plcDataObjects)
		{
			this.Product = new ProductDefinition(productFileName);
			this.ProductOrg = (ProductLib.Product)this.Product.ProductData.Clone();
			this.Product.TrySplit();
			if (!this.ProcessMachine(plcDataObjects))
			{
				return false;
			}
			if (!this.ProcessJobs())
			{
				return false;
			}
			if (!this.ProcessLayers())
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < this.layers.Count; i++)
			{
				PlcLayerDataRaw item = this.layers[i];
				if (item.CorrectionEna)
				{
					flag = true;
				}
				short index = -1;
				int num = -1;
				bool flag1 = true;
				for (int j = 0; j < this.jobs.Count; j++)
				{
					if (this.jobs[j].Layer == item.Index)
					{
						PlcProductionDataRaw plcProductionDataRaw = this.jobs[j];
						if (!flag1)
						{
							PlcProductionDataRaw item1 = this.jobs[num];
							item1.NextOnLayer = plcProductionDataRaw.Index;
							this.jobs[num] = item1;
						}
						else
						{
							item.ProductLink = plcProductionDataRaw.Index;
							item.Height = plcProductionDataRaw.Height;
							flag1 = false;
						}
						plcProductionDataRaw.Layer = item.Index;
						plcProductionDataRaw.PrevOnLayer = index;
						index = plcProductionDataRaw.Index;
						this.jobs[j] = plcProductionDataRaw;
						num = j;
					}
				}
				this.layers[i] = item;
			}
			PlcLayerDataRaw plcLayerDataRaw = this.layers.Last<PlcLayerDataRaw>();
			plcLayerDataRaw.IsLast = true;
			this.layers[this.layers.Count - 1] = plcLayerDataRaw;
			if (!flag)
			{
				return true;
			}
			this.ProcessPileCorrection();
			return true;
		}

		private PlcProductionDataRaw? ReadJob(List<ServiceDataObject> dataObjects, int index)
		{
			this.jobs = this.ReadJobs(dataObjects);
			if (index >= this.jobs.Count || index < 0)
			{
				return null;
			}
			return new PlcProductionDataRaw?(this.jobs[index]);
		}

		private List<PlcProductionDataRaw> ReadJobs(List<ServiceDataObject> dataObjects)
		{
			List<PlcProductionDataRaw> plcProductionDataRaws = new List<PlcProductionDataRaw>();
			foreach (ServiceDataObject dataObject in dataObjects)
			{
				if (!(dataObject.DataPntr.Type == "T_ProductionData") || !dataObject.DataPntr.IsArray)
				{
					continue;
				}
				foreach (object datum in dataObject.Data as List<object>)
				{
					plcProductionDataRaws.Add((PlcProductionDataRaw)datum);
				}
			}
			return plcProductionDataRaws;
		}

		private PlcLayerDataRaw? ReadLayer(List<ServiceDataObject> dataObjects, int index)
		{
			this.layers = this.ReadLayers(dataObjects);
			if (index >= this.layers.Count || index < 0)
			{
				return null;
			}
			return new PlcLayerDataRaw?(this.layers[index]);
		}

		private List<PlcLayerDataRaw> ReadLayers(List<ServiceDataObject> dataObjects)
		{
			List<PlcLayerDataRaw> plcLayerDataRaws = new List<PlcLayerDataRaw>();
			foreach (ServiceDataObject dataObject in dataObjects)
			{
				if (!(dataObject.DataPntr.Type == "T_LayerData") || !dataObject.DataPntr.IsArray)
				{
					continue;
				}
				foreach (object datum in dataObject.Data as List<object>)
				{
					plcLayerDataRaws.Add((PlcLayerDataRaw)datum);
				}
			}
			return plcLayerDataRaws;
		}

		private PlcMachineConfigurationDataRaw ReadMachineConfig(List<ServiceDataObject> dataObjects)
		{
			foreach (ServiceDataObject dataObject in dataObjects)
			{
				if (!(dataObject.DataPntr.Type == "T_ProductionMachineConfig") || dataObject.DataPntr.IsArray)
				{
					continue;
				}
				this.machineConfig = (PlcMachineConfigurationDataRaw)dataObject.Data;
			}
			return this.machineConfig;
		}

		private bool RunNcGenerator(string fileName, int layer, int todo, int done, string processPath = "")
		{
			if (!File.Exists(fileName))
			{
				return false;
			}
			string str = Path.GetFileName(fileName);
			string directoryName = Path.GetDirectoryName(fileName);
			if (!Directory.Exists(directoryName))
			{
				return false;
			}
			string str1 = string.Format("-jf=\"{0}\" -jp=\"{1}\" -cf=\"ipf.cnc\" -cp=\"Data\\Cnc\\ \" -al=\"0\" -pc=\"0\"  -layer=\"{2}\" -todo=\"{3}\" -done=\"{4}\" ", new object[] { str, directoryName, layer, todo, done });
			if (string.IsNullOrEmpty(processPath))
			{
				processPath = (new FileResources()).Data.Get<string>("ProcessPath");
				processPath = string.Concat(processPath, "CncGenerator.exe");
			}
			if (!File.Exists(processPath))
			{
				return false;
			}
			if (this.Execute(processPath, str1) == 0)
			{
				return true;
			}
			return false;
		}

		private bool RunTrafoProcess(string fileName, int layer, int todo, int done, string processPath = "")
		{
			if (!File.Exists(fileName))
			{
				return false;
			}
			string str = Path.GetFileName(fileName);
			string directoryName = Path.GetDirectoryName(fileName);
			if (!Directory.Exists(directoryName))
			{
				return false;
			}
			directoryName = string.Concat(directoryName, "\\");
			string str1 = string.Format("-c=\"startjob\" -sj  -startjobname=\"{0}\" -jobpath=\"{1}\" -layer=\"{2}\" -todo=\"{3}\" -done=\"{4}\" ", new object[] { str, directoryName, layer, todo, done });
			string str2 = processPath;
			if (!string.IsNullOrEmpty(processPath))
			{
				string str3 = processPath;
				processPath = string.Concat(str3.Remove(str3.LastIndexOf("\\")), "\\");
			}
			else
			{
				processPath = (new FileResources()).Data.Get<string>("ProcessPath");
				str2 = string.Concat(processPath, "TrafoProcess.exe");
			}
			if (!File.Exists(string.Concat(processPath, "CncGenerator.exe")))
			{
				return false;
			}
			if (!File.Exists(str2))
			{
				return false;
			}
			if (this.Execute(str2, str1) == 0)
			{
				return true;
			}
			return false;
		}

		private bool SelectFile(ref string productFileName)
		{
			productFileName = (string)System.Windows.Application.Current.Dispatcher.Invoke<object>(() => {
				string empty = string.Empty;
				OpenFileDialog openFileDialog = new OpenFileDialog()
				{
					InitialDirectory = string.Concat((new FileResources()).Data.Get<string>("ProcessPath"), "\\Data\\Products"),
					DefaultExt = ".xml",
					Filter = "Xml documents (.xml)|*.xml"
				};
				if (openFileDialog.ShowDialog(new Form()
				{
					TopMost = true,
					TopLevel = true
				}) != DialogResult.OK)
				{
					return string.Empty;
				}
				return openFileDialog.FileName;
			});
			return productFileName != string.Empty;
		}

		private List<PlcProductionDataRaw> SortJobs(List<PlcProductionDataRaw> jobs)
		{
			List<IGrouping<short, PlcProductionDataRaw>> list = (
				from x in jobs
				orderby x.Layer
				group x by x.Layer).ToList<IGrouping<short, PlcProductionDataRaw>>();
			List<PlcProductionDataRaw> plcProductionDataRaws = new List<PlcProductionDataRaw>();
			for (int i = 0; i < list.Count; i++)
			{
				List<PlcProductionDataRaw> list1 = list[i].ToList<PlcProductionDataRaw>();
				if (i % 2 != 0)
				{
					list1.Reverse();
					for (short j = 0; j < list1.Count; j = (short)(j + 1))
					{
						PlcProductionDataRaw item = list1[j];
						if (item.NextOnLayer != -1 && item.PrevOnLayer != -1)
						{
							short prevOnLayer = item.PrevOnLayer;
							item.PrevOnLayer = item.NextOnLayer;
							item.NextOnLayer = prevOnLayer;
							list1[j] = item;
						}
					}
					List<PlcProductionDataRaw> plcProductionDataRaws1 = list1;
					PlcProductionDataRaw plcProductionDataRaw = plcProductionDataRaws1[plcProductionDataRaws1.Count - 1];
					PlcLayerDataRaw fileIndex = this.layers[plcProductionDataRaw.Layer - 1];
					fileIndex.ProductLink = plcProductionDataRaw.FileIndex;
				}
				plcProductionDataRaws.AddRange(list1);
			}
			List<PlcProductionDataRaw> plcProductionDataRaws2 = new List<PlcProductionDataRaw>();
			for (short k = 0; k < plcProductionDataRaws.Count; k = (short)(k + 1))
			{
				PlcProductionDataRaw item1 = plcProductionDataRaws[k];
				item1.Index = k;
				plcProductionDataRaws2.Add(item1);
			}
			bool[] flagArray = new bool[plcProductionDataRaws2.Count];
			bool[] flagArray1 = new bool[plcProductionDataRaws2.Count];
			for (short l = 0; l < plcProductionDataRaws2.Count; l = (short)(l + 1))
			{
				PlcProductionDataRaw plcProductionDataRaw1 = plcProductionDataRaws2[l];
				for (short m = 0; m < plcProductionDataRaws2.Count; m = (short)(m + 1))
				{
					if (l != m)
					{
						PlcProductionDataRaw index = plcProductionDataRaws2[m];
						if (index.ProductFile == plcProductionDataRaw1.ProductFile)
						{
							if (flagArray[m] && index.NextOnLayer == plcProductionDataRaw1.FileIndex)
							{
								index.NextOnLayer = plcProductionDataRaw1.Index;
								flagArray[m] = true;
							}
							if (flagArray1[m] && index.PrevOnLayer == plcProductionDataRaw1.FileIndex)
							{
								index.PrevOnLayer = plcProductionDataRaw1.Index;
								flagArray1[m] = true;
							}
							plcProductionDataRaws2[m] = index;
						}
					}
				}
				PlcLayerDataRaw plcLayerDataRaw = this.layers[plcProductionDataRaw1.Layer - 1];
				if (plcLayerDataRaw.ProductLink == plcProductionDataRaw1.FileIndex)
				{
					plcLayerDataRaw.ProductLink = plcProductionDataRaw1.Index;
					this.layers[plcProductionDataRaw1.Layer - 1] = plcLayerDataRaw;
				}
			}
			return plcProductionDataRaws2;
		}

		private void WriteJobs(List<ServiceDataObject> dataObjects, List<PlcProductionDataRaw> jobs)
		{
			if (this.plcServices == null)
			{
				return;
			}
			foreach (ServiceDataObject dataObject in dataObjects)
			{
				if (dataObject.DataPntr.Type != "T_ProductionData")
				{
					continue;
				}
				if (jobs.Count <= 0)
				{
					break;
				}
				List<object> objs = new List<object>();
				foreach (PlcProductionDataRaw job in jobs)
				{
					objs.Add(job);
				}
				dataObject.Data = objs;
				this.plcServices.WriteObject(dataObject.Data, dataObject.DataPntr);
				dataObject.DataPntr.Count = (short)jobs.Count;
				this.plcServices.WriteByRef(dataObject.DataPntr, dataObject.DataPntrHandle);
				string name = dataObjects[0].telegram.CommAddress.Owner.Name;
				if (this.plcServices.IsPlcAddress(name))
				{
					name = this.plcServices.AddChannel(name);
				}
				Address address = new Address(this.Sender.Owner, name, "", null);
				this.aggregator.Publish<Telegram>(new Telegram(address, 0, null, null), true);
				return;
			}
		}

		private void WriteLayers(List<ServiceDataObject> dataObjects, List<PlcLayerDataRaw> layers)
		{
			if (this.plcServices == null)
			{
				return;
			}
			foreach (ServiceDataObject dataObject in dataObjects)
			{
				if (dataObject.DataPntr.Type != "T_LayerData")
				{
					continue;
				}
				if (this.jobs.Count <= 0)
				{
					break;
				}
				List<object> objs = new List<object>();
				foreach (PlcLayerDataRaw layer in layers)
				{
					objs.Add(layer);
				}
				dataObject.Data = objs;
				this.plcServices.WriteObject(dataObject.Data, dataObject.DataPntr);
				dataObject.DataPntr.Count = (short)layers.Count;
				this.plcServices.WriteByRef(dataObject.DataPntr, dataObject.DataPntrHandle);
				string name = dataObjects[0].telegram.CommAddress.Owner.Name;
				if (this.plcServices.IsPlcAddress(name))
				{
					name = this.plcServices.AddChannel(name);
				}
				Address address = new Address(this.Sender.Owner, name, "", null);
				this.aggregator.Publish<Telegram>(new Telegram(address, 0, null, null), true);
				return;
			}
		}

		public enum Commands
		{
			GenerateAndLoadNc = 1,
			ParseAndLoadProductFile = 2,
			Create = 3,
			InitNc = 10,
			InitProductList = 11,
			NcFault = 12,
			ProcessJobs = 20,
			RequestState = 21,
			NoJobActive = 30,
			JobActive = 31
		}
	}
}