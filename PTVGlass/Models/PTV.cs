using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PTVGlass
{
	public class Departure
	{
		public Platform Platform { get; set; }
		public Run Run { get; set; }
		public DateTime TimeTimetableUTC { get; set; }
		public DateTime? TimeRealtimeUTC { get; set; }
		public String Flags { get; set; }
	}

	public class Platform 
	{
		public int RealtimeID { get; set; }
		public Stop Stop { get; set; }
		public Direction Direction { get; set; }
	}

	public class Stop
	{
		public string Suburb {get; set;}
		public TransportType TransportType { get; set; }
		public int StopID {get; set; }
		public string LocationName {get; set;}
		public double Latitude { get; set; }
		public double Longitud { get; set; }
		public double Distance { get; set; }
	}

	public enum TransportType
	{	
		Train = 0,
		Tram = 1,
		Bus = 2,
		VLine = 3,
		Nightrider = 4
	}

	public class Direction 
	{
		public int LineDirectionID { get; set; }
		public int DirectionID { get; set; }
		public string DirectionName { get; set; }
		public Line Line { get; set; }
	}

	public class Line 
	{
		public TransportType TransportType { get; set; }
		public int LineID { get; set; }
		public string LineName { get; set; }
		public string LineNumber { get; set; }
	}

	public class Run 
	{
		public TransportType TransportType { get; set; }
		public int RunID { get; set; }
		public int NumSkipped { get; set; }
		public int DestinationId { get; set; }
		public string DestinationName { get; set; }
	}
}

