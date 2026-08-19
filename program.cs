using System;
using System.Collections.Generic;
interface Ipayable
{
    double calculatefee();
}
abstract class vehicle
{
    public string owner;
    public string brand;
    public int numberplate;
    public vehicle(string owner, string brand, int numberplate)
    {
        this.owner = owner;
        this.brand = brand;
        this.numberplate = numberplate;
    }
    public virtual void displayvehicle()
    {
        Console.WriteLine($"Vehicle owner:{owner},vehicle brand:{brand},Vehicle numberplate:{numberplate}");

    }
    public abstract string displayvehicletype();
}
class car : vehicle
{
    public int seats;
    public car(string owner, string brand, int numberplate, int seats) : base(owner, brand, numberplate)
    {
        this.seats = seats;
    }
    public override string displayvehicletype()
    {
        return "car";
    }
    public override void displayvehicle()
    {
        base.displayvehicle();
        Console.WriteLine($"vehicletype:car,seats={seats}");
    }
}

class motorcycle : vehicle
{
    public bool carrier;
    public motorcycle(string owner, string brand, int numberplate, bool carrier) : base(owner, brand, numberplate)
    {
        this.carrier = carrier;
    }
    public override string displayvehicletype()
    {
        return "motorcycle";
    }
    public override void displayvehicle()
    {
        base.displayvehicle();
        Console.WriteLine($"vehicletype:motorcycle,carrier={carrier}");
    }
}
class cng : vehicle
{
    public int seatnum;
    public cng(string owner, string brand, int numberplate, int seatnum) : base(owner, brand, numberplate)
    {
        this.seatnum = seatnum;
    }
    public override string displayvehicletype()
    {
        return "CNG";
    }
    public override void displayvehicle()
    {
        base.displayvehicle();
        Console.WriteLine($"vehicletype:CNG,seattnum={seatnum}");
    }
}
class driver
{
    public string name;
    public int phone;
    public driver()
    {
        name = "unknown";
        phone = 0;
    }
    public driver(string name, int phone)
    {
        this.name = name;
        this.phone = phone;
    }
    public void diverinfo()
    {
        Console.WriteLine($"Driver name is:{name},Driver phone :{phone}");
    }
}

class parkingslot
{
    public int slotnumber;
    public string slottype;
    public bool occupied;
    public vehicle parkedvehicle;
    public static int totalslot;
    public parkingslot(int slotnum, string Slottype, int totalslot)
    {
        slotnumber = slotnum;
        slottype = Slottype;
        occupied = false;
        parkedvehicle = null;
        parkingslot.totalslot++;
    }
    public void displayPSinfo()
    {
        Console.WriteLine($"slot:{slotnumber},Type of vehicle in slot:{slottype},currently:{occupied},parkedvehicle:{parkedvehicle}");

    }
    public void assigned(vehicle Vehicle)
    {
        parkedvehicle = Vehicle;
        occupied = true;
    }
    public void released()
    {
        parkedvehicle = null;
        occupied = false;
    }

}
class parkingticket
{
    public int ticketnum;
    public vehicle Vehicle;
    public parkingslot slot;
    public DateTime entrytime;
    public DateTime exittime;
    public bool lost;
    public static double hourlyrate;
    public parkingticket(int Ticketnum, vehicle Vehiclee, parkingslot Slot)
    {
        ticketnum = Ticketnum;
        Vehicle = Vehiclee;
        slot = Slot;
        entrytime = DateTime.Now;
        lost = false;
    }
    public parkingticket(parkingticket oldticket)
    {
        ticketnum = oldticket.ticketnum;
        Vehicle = oldticket.Vehicle;
        slot = oldticket.slot;
        entrytime = oldticket.entrytime;
        exittime = oldticket.exittime;
        lost = oldticket.lost;
    }
    public double calculatefee()
    {
        exittime = DateTime.Now;
        double hours = Math.Ceiling((exittime - entrytime).TotalHours);
        if (hours < 1)
            hours = 1;
        return hours * hourlyrate;
    }
    public double calculatefee(double customrate)
    {
        exittime = DateTime.Now;
        double hours = Math.Ceiling((exittime - entrytime).TotalHours);
        if (hours < 1)
            hours = 1;
        return hours * customrate;
    }
    public double lostticket()
    {
        return 200;
    }
    public void displayticket()
    {
        Console.WriteLine("\nparkingticket");
        Console.WriteLine($"Ticket Number={ticketnum},vehicle ={Vehicle.numberplate},Vehicletype={Vehicle.displayvehicletype()},Parking Slot:{slot.slotnumber},Entrytime={entrytime},");

    }

}
class parkingsystem
{
    public List<vehicle> vehicles;
    public List<driver> drivers;
    public List<parkingslot> slots;
    public List<parkingticket> tickets;
    public double revenue;
    public static string parkingname = "VoidBound will";
    public parkingsystem()
    {
        vehicles = new List<vehicle>();
        drivers = new List<driver>();
        slots = new List<parkingslot>();
        tickets = new List<parkingticket>();
        revenue = 0;

    }
    public static void displayname()
    {
        Console.WriteLine(parkingname);
    }
    public void registereddriver(driver driver)
    {
        drivers.Add(driver);
        Console.WriteLine("succesfully register the driver");
    }
    public void registervehicle(vehicle vehicle)
    {
        vehicles.Add(vehicle);
        Console.WriteLine("successfully registered vehicle");
    }
    public void Addslot(parkingslot slot)
    {
        slots.Add(slot);
    }
    public void Addslot(int slotsnumber)
    {
        slots.Add(new parkingslot(slotsnumber, "General", 0));
    }
    public void avaliableslot()
    {
        Console.WriteLine("\n avaliable slot:");
        bool found = false;
        foreach (parkingslot slot in slots)
        {
            if (!slot.occupied)
            {
                slot.displayPSinfo();
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("No available slots.");
        }
    }
    public parkingslot findavailableslot(string vehicleType)
    {
        foreach (parkingslot slot in slots)
        {
            if (!slot.occupied)
            {
                if (slot.slottype == vehicleType ||
                    slot.slottype == "General")
                {
                    return slot;
                }
            }
        }

        return null;
    }
    public parkingticket ParkVehicle(
        vehicle vehicle,
        int ticketNumber)
    {
        parkingslot slot =
            findavailableslot(vehicle.displayvehicletype());

        if (slot == null)
        {
            Console.WriteLine("No suitable parking slot available");
            return null;
        }

        slot.assigned(vehicle);

        parkingticket ticket =
            new parkingticket(ticketNumber, vehicle, slot);

        tickets.Add(ticket);

        Console.WriteLine("\nVehicle parked successfully.");

        ticket.displayticket();
        return ticket;
    }
    public void exitvehicle(parkingticket ticket)
    {
        if (ticket == null)
        {
            Console.WriteLine("Invalid ticket");
            return;
        }

        double fee;

        if (ticket.lost)
        {
            fee = ticket.lostticket();
            Console.WriteLine("\nlost ticket");
            Console.WriteLine($"lost ticket fee: {fee} Tk");
        }
        else
        {
            fee = ticket.calculatefee();

            Console.WriteLine("\nVEHICLE EXIT :");
            Console.WriteLine($"Vehicle: {ticket.Vehicle.numberplate}");
            Console.WriteLine($"Slot: {ticket.slot.slotnumber}");
            Console.WriteLine($"Parking Fee: {fee} Tk");
        }

        revenue += fee;

        ticket.slot.released();

        Console.WriteLine("Parking slot released.");
    }

    public void handlelostticket(parkingticket ticket)
    {
        if (ticket != null)
        {
            ticket.lost = true;
            Console.WriteLine("Ticket marked as lost");
        }
    }
    public void generatedailyreport()
    {
        Console.WriteLine("\nDAILY PARKING REPORT :");

        Console.WriteLine($"Total registered Drivers: {drivers.Count},Total Registered vehicles: {vehicles.Count},Total Parking Slots: {slots.Count},Total tickets: {tickets.Count},Total revenue: {revenue} Tk");

        int occupied = 0;

        foreach (parkingslot slot in slots)
        {
            if (slot.occupied)
                occupied++;
        }

        Console.WriteLine($"Occupied Slots: {occupied},Available Slots: {slots.Count - occupied}");
    }
}
class program
{
    class Program
    {
        static void Main(string[] args)
        {
            parkingsystem system = new parkingsystem();
            parkingsystem.displayname();

            driver driver1 =
                new driver("Arifa", 01700000000);

            driver driver2 =
                new driver("munni", 67890);

            system.registereddriver(driver1);
            system.registereddriver(driver2);

            car car =
                new car("Arifa", "Toyota", 1234, 5);

            motorcycle motorcycle =
                new motorcycle("munni", "Yamaha", 5678, true);

            cng cng =
                new cng("nusrat", "Bajaj", 9999, 3);

            system.registervehicle(car);
            system.registervehicle(motorcycle);
            system.registervehicle(cng);

            Console.WriteLine("\nVEHICLE INFORMATION:");

            car.displayvehicle();

            Console.WriteLine();
            motorcycle.displayvehicle();

            Console.WriteLine();

            cng.displayvehicle();

            system.Addslot(new parkingslot(1, "car", 0));
            system.Addslot(new parkingslot(2, "motorcycle", 0));
            system.Addslot(new parkingslot(3, "cng", 0));
            system.Addslot(4);

            system.avaliableslot();
            system.avaliableslot();

            parkingticket ticket1 =
                system.ParkVehicle(car, 1001);

            parkingticket ticket2 =
                system.ParkVehicle(motorcycle, 1002);

            Console.WriteLine("\n OPERATOR OVERLOADING :");

            parkingticket copiedTicket =
                new parkingticket(ticket1);

            Console.WriteLine(
                $"Original Ticket: {ticket1.ticketnum}");

            Console.WriteLine($"Copied Ticket: {copiedTicket.ticketnum}");

            Console.WriteLine("\nMETHOD OVERLOADING:");

            Console.WriteLine(
                $"Normal Fee: {ticket1.calculatefee()} Tk");

            Console.WriteLine(
                $"Custom Rate Fee: {ticket1.calculatefee(100)} Tk");

            system.handlelostticket(ticket2);

            system.exitvehicle(ticket2);

            system.exitvehicle(ticket1);
            system.handlelostticket(ticket1);

            system.generatedailyreport();

            Console.ReadLine();
        }
    }
}
















