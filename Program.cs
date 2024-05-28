using System;
using System.Xml;
using System.Collections.Generic;
namespace XML_Prac;

public class FoodDetails
{
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public int Calories { get; set; }

    // public void FoodDetailsReadFromXML(XmlReader foodDetailsReader)
    // {
    //     foodDetailsReader.ReadToFollowing("name");
    //     Name = foodDetailsReader.ReadElementContentAsString();
    //     foodDetailsReader.ReadToFollowing("price");
    //     Price = double.Parse(foodDetailsReader.ReadElementContentAsString().Trim('$'));
    //     foodDetailsReader.ReadToFollowing("description");
    //     Description = foodDetailsReader.ReadElementContentAsString();
    //     foodDetailsReader.ReadToFollowing("calories");
    //     Calories = int.Parse(foodDetailsReader.ReadElementContentAsString());
    // }

    // public void FoodDetailsWriteToXML(XmlWriter writer)
    // {
    //     writer.WriteElementString("name", Name);
    //     writer.WriteElementString("price", $"${Price}");
    //     writer.WriteElementString("description", Description);
    //     writer.WriteElementString("calories", $"{Calories}");
    // }
}
public class Food
{
    public int OrderID { get; set; }
    public FoodDetails FoodDetails { get; set; } = new FoodDetails();

    // public void FoodReadFromXML(XmlReader foodReader)
    // {
    //     if (foodReader.HasAttributes)
    //     {
    //         OrderID = int.Parse(foodReader.GetAttribute("orderid"));
    //         FoodDetails.FoodDetailsReadFromXML(foodReader);

    //     }
    //     else
    //     {
    //         return;
    //     }
    // }

    // public void FoodWriteToXML(XmlWriter writer)
    // {
    //     writer.WriteStartElement("food");
    //     writer.WriteAttributeString("orderid", OrderID.ToString());
    //     FoodDetails.FoodDetailsWriteToXML(writer);
    //     writer.WriteEndElement();
    // }
}
public class LunchMenu
{
    public List<Food> Foods { get; set; } = new List<Food>();

    // public void LunchMenuWriteToXML(XmlWriter writer)
    // {
    //     writer.WriteStartElement("lunch_menu");
    //     foreach (Food food in Foods)
    //     {
    //         food.FoodWriteToXML(writer);
    //     }
    //     writer.WriteEndElement();
    // }
}

class Program
{
    static LunchMenu lunchMenu = new LunchMenu();
    public static void Main(string[] args)
    {
        string filePath = "FoodCatelog3.xml"; // Path to your XML file
        ReadFromXMLFile(filePath);
        string outputFile = "output.xml"; //path to output file
        WriteToXmlFile(outputFile);

    }

    //reading xml
    //read form Food details
    public static FoodDetails FoodDetailsReadFromXML(XmlReader foodDetailsReader)
    {
        FoodDetails foodDetails = new FoodDetails();
        foodDetailsReader.ReadToFollowing("name");
        foodDetails.Name = foodDetailsReader.ReadElementContentAsString();
        foodDetailsReader.ReadToFollowing("price");
        foodDetails.Price = double.Parse(foodDetailsReader.ReadElementContentAsString().Trim('$'));
        foodDetailsReader.ReadToFollowing("description");
        foodDetails.Description = foodDetailsReader.ReadElementContentAsString();
        foodDetailsReader.ReadToFollowing("calories");
        foodDetails.Calories = int.Parse(foodDetailsReader.ReadElementContentAsString());

        return foodDetails;
    }

    //read from Food
    public static Food FoodReadFromXML(XmlReader foodReader)
    {
        Food food = new Food();
        if (foodReader.HasAttributes)
        {
            food.OrderID = int.Parse(foodReader.GetAttribute("orderid"));
            food.FoodDetails = FoodDetailsReadFromXML(foodReader);
        }
        return food;
    }

    //read from Lunch menu
    public static void LunchMenuReadFromXML(XmlReader reader)
    {
        while (reader.ReadToFollowing("food"))
        {
            Food food = FoodReadFromXML(reader);
            lunchMenu.Foods.Add(food);
        }
    }

    //reading from xml file
    public static void ReadFromXMLFile(string path)
    {
        using (XmlReader reader = XmlReader.Create(path))
        {
            reader.ReadToFollowing("lunch_menu");
            LunchMenuReadFromXML(reader);
        }

        System.Console.WriteLine("success");

        foreach (Food food in lunchMenu.Foods)
        {
            Console.WriteLine($"OrderId: {food.OrderID}, Name: {food.FoodDetails.Name}, Price: {food.FoodDetails.Price}, Description: {food.FoodDetails.Description}, Calories: {food.FoodDetails.Calories}");
        }

    }

    //writing XML
    //writing Food Details Data
    public static void FoodDetailsWriteToXML(XmlWriter writer,FoodDetails foodDetails)
    {
        writer.WriteElementString("name", foodDetails.Name);
        writer.WriteElementString("price", $"${foodDetails.Price}");
        writer.WriteElementString("description", foodDetails.Description);
        writer.WriteElementString("calories", $"{foodDetails.Calories}");
    }
    //writing Food Data
    public static void FoodWriteToXML(XmlWriter writer, Food food)
    {
        writer.WriteStartElement("food");
        writer.WriteAttributeString("orderid", food.OrderID.ToString());
        FoodDetailsWriteToXML(writer, food.FoodDetails);
        writer.WriteEndElement();
    }

    //writing Lunch menu Data
    public static void LunchMenuWriteToXML(XmlWriter writer)
    {
        writer.WriteStartElement("lunch_menu");
        foreach (Food food in lunchMenu.Foods)
        {
            FoodWriteToXML(writer,food);
        }
        writer.WriteEndElement();
    }


    //writing to Xml file
    public static void WriteToXmlFile(string outputFile)
    {
        XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true
        };

        using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
        {
            LunchMenuWriteToXML(writer);
        }
    }

}
