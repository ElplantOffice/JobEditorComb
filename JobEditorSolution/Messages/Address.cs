
using System;

namespace Messages
{
  [Serializable]
  public class Address : IAddress
  {
    private const int BASEPART = 0;

    public string Owner { get; private set; }

    public string Target { get; private set; }

    public string Relay { get; private set; }

    public Address(string owner, string target, string relay, string addChildOwner, string addChildTarget)
    {
      this.Owner = owner;
      this.Target = target;
      this.Relay = relay;
      if (!string.IsNullOrWhiteSpace(addChildOwner) && !string.IsNullOrWhiteSpace(this.Owner))
        this.Owner = this.Owner + "." + addChildOwner;
      if (string.IsNullOrWhiteSpace(addChildTarget) || string.IsNullOrWhiteSpace(this.Target))
        return;
      this.Target = this.Target + "." + addChildTarget;
    }

    public Address(string owner, string target, string relay = "", string addChild = null)
    {
      this.Owner = owner;
      this.Target = target;
      this.Relay = relay;
      if (!string.IsNullOrWhiteSpace(addChild) && !string.IsNullOrWhiteSpace(this.Owner))
        this.Owner = this.Owner + "." + addChild;
      if (string.IsNullOrWhiteSpace(addChild) || string.IsNullOrWhiteSpace(this.Target))
        return;
      this.Target = this.Target + "." + addChild;
    }

    public Address(IAddress parent, string addChild = null)
    {
      this.Owner = parent.Owner;
      this.Target = parent.Target;
      this.Relay = parent.Relay;
      if (!string.IsNullOrWhiteSpace(addChild) && !string.IsNullOrWhiteSpace(this.Owner))
        this.Owner = this.Owner + "." + addChild;
      if (!string.IsNullOrWhiteSpace(addChild) && !string.IsNullOrWhiteSpace(this.Target))
        this.Target = this.Target + "." + addChild;
      if (string.IsNullOrWhiteSpace(addChild) || string.IsNullOrWhiteSpace(this.Relay))
        return;
      this.Relay = this.Relay + "." + addChild;
    }

    public Address(IAddress parent, Type childType)
    {
      this.Owner = parent.Owner + "." + childType.Name;
      this.Target = parent.Target;
    }

    private string GetAddressPart(int index, string inputString, string separator = ".")
    {
      if (inputString == null)
        return (string) null;
      string[] separator1 = new string[1]{ separator };
      string[] strArray = inputString.Split(separator1, StringSplitOptions.None);
      if (index < strArray.Length && index >= 0)
        return strArray[index];
      return (string) null;
    }

    public string GetOwnerBasePart()
    {
      return this.GetAddressPart(0, this.Owner, ".");
    }

    public string GetTargetBasePart()
    {
      return this.GetAddressPart(0, this.Target, ".");
    }

    public Address GetBasePart(string addChild = null)
    {
      return new Address(this.GetOwnerBasePart(), this.GetTargetBasePart(), "", addChild);
    }

    public Address GetBasePart(Type childType)
    {
      return new Address(this.GetOwnerBasePart(), this.GetTargetBasePart(), "", childType.Name);
    }

    public Address GetParent()
    {
      return new Address(this.Owner.Remove(this.Owner.LastIndexOf(".")), (string) null, "", (string) null);
    }
  }
}
