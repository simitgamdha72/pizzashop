using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class User
{
  

    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? UserName { get; set; }

    public int? Phone { get; set; }

    public string? Country { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    public string? Address { get; set; }

    public int? Zipcode { get; set; }

    public byte[]? Image { get; set; }

    public int? RoleId { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public bool? Isdeleted { get; set; }

    public virtual ICollection<AssignTable> AssignTableCreatedByNavigations { get; set; } = new List<AssignTable>();

    public virtual ICollection<AssignTable> AssignTableModifiedByNavigations { get; set; } = new List<AssignTable>();

    public virtual ICollection<Category> CategoryCreatedByNavigations { get; set; } = new List<Category>();

    public virtual ICollection<Category> CategoryModifiedByNavigations { get; set; } = new List<Category>();

    public virtual ICollection<City> CityCreatedByNavigations { get; set; } = new List<City>();

    public virtual ICollection<City> CityModifiedByNavigations { get; set; } = new List<City>();

    public virtual ICollection<Country> CountryCreatedByNavigations { get; set; } = new List<Country>();

    public virtual ICollection<Country> CountryModifiedByNavigations { get; set; } = new List<Country>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Customer> CustomerCreatedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<CustomerHistory> CustomerHistoryCreatedByNavigations { get; set; } = new List<CustomerHistory>();

    public virtual ICollection<CustomerHistory> CustomerHistoryModifiedByNavigations { get; set; } = new List<CustomerHistory>();

    public virtual ICollection<Customer> CustomerModifiedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<User> InverseCreatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseModifiedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<Invoice> InvoiceCreatedByNavigations { get; set; } = new List<Invoice>();

    public virtual ICollection<Invoice> InvoiceModifiedByNavigations { get; set; } = new List<Invoice>();

    public virtual ICollection<Itemmodifier> ItemmodifierCreatedByNavigations { get; set; } = new List<Itemmodifier>();

    public virtual ICollection<Itemmodifier> ItemmodifierModifiedByNavigations { get; set; } = new List<Itemmodifier>();

    public virtual ICollection<MenuItem> MenuItemCreatedByNavigations { get; set; } = new List<MenuItem>();

    public virtual ICollection<MenuItem> MenuItemModifiedByNavigations { get; set; } = new List<MenuItem>();

    public virtual ICollection<MenuModifier> MenuModifierCreatedByNavigations { get; set; } = new List<MenuModifier>();

    public virtual ICollection<MenuModifier> MenuModifierModifiedByNavigations { get; set; } = new List<MenuModifier>();

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<ModifiedOrderDetail> ModifiedOrderDetailCreatedByNavigations { get; set; } = new List<ModifiedOrderDetail>();

    public virtual ICollection<ModifiedOrderDetail> ModifiedOrderDetailModifiedByNavigations { get; set; } = new List<ModifiedOrderDetail>();

    public virtual ICollection<ModifiersGroup> ModifiersGroupCreatedByNavigations { get; set; } = new List<ModifiersGroup>();

    public virtual ICollection<ModifiersGroup> ModifiersGroupModifiedByNavigations { get; set; } = new List<ModifiersGroup>();

    public virtual ICollection<Order> OrderCreatedByNavigations { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderModifiedByNavigations { get; set; } = new List<Order>();

    public virtual ICollection<OrderTax> OrderTaxCreatedByNavigations { get; set; } = new List<OrderTax>();

    public virtual ICollection<OrderTax> OrderTaxModifiedByNavigations { get; set; } = new List<OrderTax>();

    public virtual ICollection<Ordersitemdetail> OrdersitemdetailCreatedByNavigations { get; set; } = new List<Ordersitemdetail>();

    public virtual ICollection<Ordersitemdetail> OrdersitemdetailModifiedByNavigations { get; set; } = new List<Ordersitemdetail>();

    public virtual ICollection<Payment> PaymentCreatedByNavigations { get; set; } = new List<Payment>();

    public virtual ICollection<Payment> PaymentModifiedByNavigations { get; set; } = new List<Payment>();

    public virtual ICollection<Permission> PermissionCreatedByNavigations { get; set; } = new List<Permission>();

    public virtual ICollection<Permission> PermissionModifiedByNavigations { get; set; } = new List<Permission>();

    public virtual ICollection<Rating> RatingCreatedByNavigations { get; set; } = new List<Rating>();

    public virtual ICollection<Rating> RatingModifiedByNavigations { get; set; } = new List<Rating>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Role> RoleCreatedByNavigations { get; set; } = new List<Role>();

    public virtual ICollection<Role> RoleModifiedByNavigations { get; set; } = new List<Role>();

    public virtual ICollection<Roleswisepermission> RoleswisepermissionCreatedByNavigations { get; set; } = new List<Roleswisepermission>();

    public virtual ICollection<Roleswisepermission> RoleswisepermissionModifiedByNavigations { get; set; } = new List<Roleswisepermission>();

    public virtual ICollection<Section> SectionCreatedByNavigations { get; set; } = new List<Section>();

    public virtual ICollection<Section> SectionModifiedByNavigations { get; set; } = new List<Section>();

    public virtual ICollection<State> StateCreatedByNavigations { get; set; } = new List<State>();

    public virtual ICollection<State> StateModifiedByNavigations { get; set; } = new List<State>();

    public virtual ICollection<Table> TableCreatedByNavigations { get; set; } = new List<Table>();

    public virtual ICollection<Table> TableModifiedByNavigations { get; set; } = new List<Table>();

    public virtual ICollection<Tableorder> TableorderCreatedByNavigations { get; set; } = new List<Tableorder>();

    public virtual ICollection<Tableorder> TableorderModifiedByNavigations { get; set; } = new List<Tableorder>();

    public virtual ICollection<TaxesFee> TaxesFeeCreatedByNavigations { get; set; } = new List<TaxesFee>();

    public virtual ICollection<TaxesFee> TaxesFeeModifiedByNavigations { get; set; } = new List<TaxesFee>();

    public virtual ICollection<Token> TokenCreatedByNavigations { get; set; } = new List<Token>();

    public virtual ICollection<Token> TokenModifiedByNavigations { get; set; } = new List<Token>();

    public virtual ICollection<Unit> UnitCreatedByNavigations { get; set; } = new List<Unit>();

    public virtual ICollection<Unit> UnitModifiedByNavigations { get; set; } = new List<Unit>();

    public virtual ICollection<WaitingList> WaitingListCreatedByNavigations { get; set; } = new List<WaitingList>();

    public virtual ICollection<WaitingList> WaitingListModifiedByNavigations { get; set; } = new List<WaitingList>();
}
