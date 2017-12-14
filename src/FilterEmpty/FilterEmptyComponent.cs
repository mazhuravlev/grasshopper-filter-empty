using System;
using System.Collections.Generic;
using FilterEmpty.Properties;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace FilterEmpty
{
  public class FilterEmptyComponent : GH_Component
  {
    private int _in;
    private int _out;
    private int _indexOut;
    private int _maskOut;
    private int _nullOut;
    public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    public FilterEmptyComponent()
      : base("Filter Empty", "FE",
          "Filters <empty> elements from list",
          "Sets", "List")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      _in = pManager.AddGenericParameter("Input", "I", "List input", GH_ParamAccess.list);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      _out = pManager.AddGenericParameter("Filtered", "F", "Filtered list", GH_ParamAccess.list);
      _indexOut = pManager.AddIntegerParameter("Empty indexes", "Ei", "Empty indexes", GH_ParamAccess.list);
      _maskOut = pManager.AddBooleanParameter("Is Empty", "E", "Is element <empty>", GH_ParamAccess.list);
      _nullOut = pManager.AddGenericParameter("Nulls", "N", "Replace <empty> with nulls", GH_ParamAccess.list);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object can be used to retrieve data from input parameters and 
    /// to store data in output parameters.</param>
    protected override void SolveInstance(IGH_DataAccess da)
    {
      var list = new List<object>();
      da.GetDataList(_in, list);
      var result = new List<object>();
      var mask = new List<bool>();
      var indexes = new List<int>();
      var nullReplaced = new List<object>();
      for (var i = 0; i < list.Count; i++)
      {
        // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
        var isEmpty = list[i] is GH_Goo<string> && string.IsNullOrEmpty(((GH_Goo<string>)list[i]).Value);
        if(isEmpty) indexes.Add(i);
        mask.Add(isEmpty);
        if(!isEmpty)result.Add(list[i]);
        nullReplaced.Add(isEmpty ? null : list[i]);
      }
      da.SetDataList(_out, result);
      da.SetDataList(_indexOut, indexes);
      da.SetDataList(_maskOut, mask);
      da.SetDataList(_nullOut, nullReplaced);
    }

    /// <summary>
    /// Provides an Icon for every component that will be visible in the User Interface.
    /// Icons need to be 24x24 pixels.
    /// </summary>
    protected override System.Drawing.Bitmap Icon => Resources.icon;

    /// <summary>
    /// Each component must have a unique Guid to identify it. 
    /// It is vital this Guid doesn't change otherwise old ghx files 
    /// that use the old ID will partially fail during loading.
    /// </summary>
    public override Guid ComponentGuid => new Guid("75c4376f-3f15-421b-9e11-843bdaafd7ca");
  }
}
