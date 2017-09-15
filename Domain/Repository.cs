using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Repository
    {
        private string _connectionString;

        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool IsUserInRole(int userId, string roleName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                SqlParameter uId = new SqlParameter("@userId", userId); command.Parameters.Add(uId);
                SqlParameter role = new SqlParameter("@role", roleName); command.Parameters.Add(role);

                command.CommandText =
                @"select top 1 1 from webpages_UsersInRoles ur
                INNER JOIN webpages_Roles r ON ur.RoleId = r.RoleId
                WHERE r.RoleName = @role
                AND ur.UserId = @userId";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                return reader.HasRows;
            }
        }

        public List<Bid> ListBid(bool admin)
        {
            List<Bid> list = new List<Bid>();
            string whereClause = String.Empty;

            if (admin)
            {
                whereClause = "[bid_end] > GETDATE() AND bid.deleted=0";
            }
            else
            {
                whereClause = "[bid_end] > GETDATE() AND bid.deleted=0";
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = @"SELECT [bid_id]
                    ,bid.[UserId]
                    ,bid.[request_info_id]
                    ,[amount]
                    ,bid.[description]
                    ,[reference_number]
                    ,[accepted]
                    ,bid.[document_info]
                    ,bid.[approved]

                    ,company.[company_name]
                    ,company.[contact_name]
                    ,company.[street_address]
                    ,company.[city]
                    ,company.[state_province_code]
                    ,company.[country_code]
                    ,company.[postal_code]
                    ,company.[phone]
                    ,company.[mobile]
                    ,company.[fax]
                    ,company.[email]
      
                  FROM [minebidz_iias_ca_sql].[dbo].[bid] bid
                  INNER JOIN
                  contact_info company ON company.contact_id = bid.contact_info_id
                  INNER JOIN 
                  request_info ri ON bid.request_info_id = ri.request_info_id
                  INNER JOIN 
                  bid_info bi ON bi.bid_info_id = ri.bid_info_id
                  WHERE ";
                command.CommandText += whereClause;
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(
                    new Bid
                    {
                        Id = reader.GetInt32(0),
                        UserId = (int)reader["UserId"],
                        Amount = (decimal)reader["amount"],
                        Description = reader["description"] is DBNull ? String.Empty : (string)reader["description"],
                        EngineeringDesign = reader["document_info"] is DBNull ? String.Empty : (string)reader["document_info"],
                        ReferenceNumber = reader["reference_number"] is DBNull ? String.Empty : (string)reader["reference_number"],
                        Accepted = (bool)reader["accepted"],
                        Approved = (bool)reader["approved"],
                        RequestInfoId = (int)reader["request_info_id"],

                        CompanyInfo = new ContactInfo()
                        {
                            City = reader["city"] is DBNull ? String.Empty : (string)reader["city"],
                            CompanyName = reader["company_name"] is DBNull ? String.Empty : (string)reader["company_name"],
                            ContactName = reader["contact_name"] is DBNull ? String.Empty : (string)reader["contact_name"],
                            CountryCode = reader["country_code"] is DBNull ? String.Empty : (string)reader["country_code"],
                            Email = reader["email"] is DBNull ? String.Empty : (string)reader["email"],
                            Fax = reader["fax"] is DBNull ? String.Empty : (string)reader["fax"],
                            Mobile = reader["mobile"] is DBNull ? String.Empty : (string)reader["mobile"],
                            Phone = reader["phone"] is DBNull ? String.Empty : (string)reader["phone"],
                            PostalCode = reader["postal_code"] is DBNull ? String.Empty : (string)reader["postal_code"],
                            ProvinceStateCode = reader["state_province_code"] is DBNull ? String.Empty : (string)reader["state_province_code"],
                            StreetAddress = reader["street_address"] is DBNull ? String.Empty : (string)reader["street_address"]
                        },
                    });
                }
            }
            return list;
        }


        public List<RequestInfo> ListRequestInfo()
        {
            List<Condition> conditionList = ListCondition();
            List<RequestInfo> list = new List<RequestInfo>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = @"SELECT [request_info_id]
                      ,[UserId]
                      ,[details_info]
                      ,[approved]
                        ,[class_name]
                        ,[category_id]
                        ,[subcategory_id]
      
                      ,company.[company_name]
                      ,company.[contact_name]
                      ,company.[street_address]
                      ,company.[city]
                      ,company.[state_province_code]
                      ,company.[country_code]
                      ,company.[postal_code]
                      ,company.[phone]
                      ,company.[mobile]
                      ,company.[fax]
                      ,company.[email]
      
                      ,delivery.[company_name] as d_company_name
                      ,delivery.[contact_name]as d_contact_name
                      ,delivery.[street_address] as d_street_address
                      ,delivery.[city] as d_city
                      ,delivery.[state_province_code] as d_state_province_code
                      ,delivery.[country_code] as d_country_code
                      ,delivery.[postal_code] as d_postal_code
                      ,delivery.[phone] as d_phone
                      ,delivery.[mobile] as d_mobile
                      ,delivery.[fax] as d_fax
                      ,delivery.[email] as d_email
                      ,[bid_name]
                      ,[bid_end]
                      ,[bid_start]
                  FROM [minebidz_iias_ca_sql].[dbo].[request_info] ri
                  INNER JOIN
                  contact_info company ON company.contact_id = ri.company_contact_id
                  INNER JOIN
                  contact_info delivery ON delivery.contact_id = ri.delivery_contact_id
                  INNER JOIN 
                  bid_info bi ON bi.bid_info_id = ri.bid_info_id
                  WHERE approved = 1 AND [bid_end] > GETDATE() AND deleted=0";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(
                    new RequestInfo
                    {
                        Id = reader.GetInt32(0),
                        UserId = (int)reader["UserId"],
                        CategoryId = (int)reader["category_id"],
                        SubcategoryId = (int)reader["subcategory_id"],
                        DetailsInfoJson = (string)reader["details_info"],
                        ClassName = (string)reader["class_name"],
                        Approved = (bool)reader["approved"], 
                        BidInfo = new BidInfo()
                        {
                            BidName =  reader["bid_name"] is DBNull ? String.Empty: (string)reader["bid_name"],
                            BidEndDate = (DateTime)reader["bid_end"],
                            BidStartDate = (DateTime)reader["bid_start"]
                        },
                        CompanyInfo = new ContactInfo()
                        {
                            City = reader["city"] is DBNull ? String.Empty : (string)reader["city"],
                            CompanyName = reader["company_name"] is DBNull ? String.Empty : (string)reader["company_name"],
                            ContactName = reader["contact_name"] is DBNull ? String.Empty : (string)reader["contact_name"],
                            CountryCode = reader["country_code"] is DBNull ? String.Empty : (string)reader["country_code"],
                            Email = reader["email"] is DBNull ? String.Empty : (string)reader["email"],
                            Fax = reader["fax"] is DBNull ? String.Empty : (string)reader["fax"],
                            Mobile = reader["mobile"] is DBNull ? String.Empty : (string)reader["mobile"],
                            Phone = reader["phone"] is DBNull ? String.Empty : (string)reader["phone"],
                            PostalCode = reader["postal_code"] is DBNull ? String.Empty : (string)reader["postal_code"],
                            ProvinceStateCode = reader["state_province_code"] is DBNull ? String.Empty : (string)reader["state_province_code"],
                            StreetAddress = reader["street_address"] is DBNull ? String.Empty : (string)reader["street_address"]
                        },
                        DeliveryInfo = new ContactInfo()
                        {
                            City = reader["d_city"] is DBNull ? String.Empty : (string)reader["d_city"],
                            CompanyName = reader["d_company_name"] is DBNull ? String.Empty : (string)reader["d_company_name"],
                            ContactName = reader["d_contact_name"] is DBNull ? String.Empty : (string)reader["d_contact_name"],
                            CountryCode = reader["d_country_code"] is DBNull ? String.Empty : (string)reader["d_country_code"],
                            Email = reader["d_email"] is DBNull ? String.Empty : (string)reader["d_email"],
                            Fax = reader["d_fax"] is DBNull ? String.Empty : (string)reader["d_fax"],
                            Mobile = reader["d_mobile"] is DBNull ? String.Empty : (string)reader["d_mobile"],
                            Phone = reader["d_phone"] is DBNull ? String.Empty : (string)reader["d_phone"],
                            PostalCode = reader["d_postal_code"] is DBNull ? String.Empty : (string)reader["d_postal_code"],
                            ProvinceStateCode = reader["d_state_province_code"] is DBNull ? String.Empty : (string)reader["d_state_province_code"],
                            StreetAddress = reader["d_street_address"] is DBNull ? String.Empty : (string)reader["d_street_address"]
                        },
                        ConditionList = new List<Condition>()
                        //Image = reader[2] == null? "": reader.GetString(2)
                    });
                }
                command.CommandText = @"SELECT ci.request_info_id
                          ,[s_condition_id]
                      FROM [minebidz_iias_ca_sql].[dbo].[condition_info] ci
                    INNER JOIN request_info ri ON ri.request_info_id = ci.request_info_id
                    INNER JOIN bid_info bi ON bi.bid_info_id = ri.bid_info_id WHERE ri.approved = 1 AND bi.bid_end > GETDATE()
                ";

                reader.Close();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int request_info_id = reader.GetInt32(0);
                    int s_condition_id = reader.GetInt32(1);
                    list.FirstOrDefault(r => r.Id == request_info_id).ConditionList.Add(conditionList.FirstOrDefault(c => c.Id == s_condition_id));
                }

            }
            return list;
        }

        public List<RequestInfo> ListRequestInfoAdmin()
        {
            List<Condition> conditionList = ListCondition();
            List<RequestInfo> list = new List<RequestInfo>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = @"SELECT [request_info_id]
                    ,[UserId]
                    ,[details_info]
                    ,[approved]
                    ,[class_name]
                    ,[category_id]
                    ,[subcategory_id]
                    ,[description]
                    ,[document_info]                        
                    ,[approved]

                    ,company.[company_name]
                    ,company.[contact_name]
                    ,company.[street_address]
                    ,company.[city]
                    ,company.[state_province_code]
                    ,company.[country_code]
                    ,company.[postal_code]
                    ,company.[phone]
                    ,company.[mobile]
                    ,company.[fax]
                    ,company.[email]
      
                    ,delivery.[company_name] as d_company_name
                    ,delivery.[contact_name]as d_contact_name
                    ,delivery.[street_address] as d_street_address
                    ,delivery.[city] as d_city
                    ,delivery.[state_province_code] as d_state_province_code
                    ,delivery.[country_code] as d_country_code
                    ,delivery.[postal_code] as d_postal_code
                    ,delivery.[phone] as d_phone
                    ,delivery.[mobile] as d_mobile
                    ,delivery.[fax] as d_fax
                    ,delivery.[email] as d_email
                    ,[bid_name]
                    ,[bid_end]
                    ,[bid_start]
                    FROM [minebidz_iias_ca_sql].[dbo].[request_info] ri
                    INNER JOIN
                    contact_info company ON company.contact_id = ri.company_contact_id
                    INNER JOIN
                    contact_info delivery ON delivery.contact_id = ri.delivery_contact_id
                    INNER JOIN 
                    bid_info bi ON bi.bid_info_id = ri.bid_info_id
                    WHERE deleted = 0 AND [bid_end] > GETDATE()";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(
                    new RequestInfo
                    {
                        Id = reader.GetInt32(0),
                        UserId = (int)reader["UserId"],
                        CategoryId = (int)reader["category_id"],
                        SubcategoryId = (int)reader["subcategory_id"],
                        DetailsInfoJson = (string)reader["details_info"],
                        ClassName = (string)reader["class_name"],
                        Approved = (bool)reader["approved"],
                        Description = reader["description"] is DBNull ? String.Empty : (string)reader["description"],
                        DocumentInfo = reader["document_info"] is DBNull ? String.Empty : (string)reader["document_info"],
                         
                        BidInfo = new BidInfo()
                        {
                            BidName = reader["bid_name"] is DBNull ? String.Empty : (string)reader["bid_name"],
                            BidEndDate = (DateTime)reader["bid_end"],
                            BidStartDate = (DateTime)reader["bid_start"]
                        },
                        CompanyInfo = new ContactInfo()
                        {
                            City = reader["city"] is DBNull ? String.Empty : (string)reader["city"],
                            CompanyName = reader["company_name"] is DBNull ? String.Empty : (string)reader["company_name"],
                            ContactName = reader["contact_name"] is DBNull ? String.Empty : (string)reader["contact_name"],
                            CountryCode = reader["country_code"] is DBNull ? String.Empty : (string)reader["country_code"],
                            Email = reader["email"] is DBNull ? String.Empty : (string)reader["email"],
                            Fax = reader["fax"] is DBNull ? String.Empty : (string)reader["fax"],
                            Mobile = reader["mobile"] is DBNull ? String.Empty : (string)reader["mobile"],
                            Phone = reader["phone"] is DBNull ? String.Empty : (string)reader["phone"],
                            PostalCode = reader["postal_code"] is DBNull ? String.Empty : (string)reader["postal_code"],
                            ProvinceStateCode = reader["state_province_code"] is DBNull ? String.Empty : (string)reader["state_province_code"],
                            StreetAddress = reader["street_address"] is DBNull ? String.Empty : (string)reader["street_address"]
                        },
                        DeliveryInfo = new ContactInfo()
                        {
                            City = reader["d_city"] is DBNull ? String.Empty : (string)reader["d_city"],
                            CompanyName = reader["d_company_name"] is DBNull ? String.Empty : (string)reader["d_company_name"],
                            ContactName = reader["d_contact_name"] is DBNull ? String.Empty : (string)reader["d_contact_name"],
                            CountryCode = reader["d_country_code"] is DBNull ? String.Empty : (string)reader["d_country_code"],
                            Email = reader["d_email"] is DBNull ? String.Empty : (string)reader["d_email"],
                            Fax = reader["d_fax"] is DBNull ? String.Empty : (string)reader["d_fax"],
                            Mobile = reader["d_mobile"] is DBNull ? String.Empty : (string)reader["d_mobile"],
                            Phone = reader["d_phone"] is DBNull ? String.Empty : (string)reader["d_phone"],
                            PostalCode = reader["d_postal_code"] is DBNull ? String.Empty : (string)reader["d_postal_code"],
                            ProvinceStateCode = reader["d_state_province_code"] is DBNull ? String.Empty : (string)reader["d_state_province_code"],
                            StreetAddress = reader["d_street_address"] is DBNull ? String.Empty : (string)reader["d_street_address"]
                        },
                        ConditionList = new List<Condition>()
                        //Image = reader[2] == null? "": reader.GetString(2)
                    });
                }
                command.CommandText = @"SELECT ci.request_info_id
                          ,[s_condition_id]
                      FROM [minebidz_iias_ca_sql].[dbo].[condition_info] ci
                    INNER JOIN request_info ri ON ri.request_info_id = ci.request_info_id
                    INNER JOIN bid_info bi ON bi.bid_info_id = ri.bid_info_id WHERE ri.approved = 1 AND bi.bid_end > GETDATE()
                ";

                reader.Close();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int request_info_id = reader.GetInt32(0);
                    int s_condition_id = reader.GetInt32(1);
                    list.FirstOrDefault(r => r.Id == request_info_id).ConditionList.Add(conditionList.FirstOrDefault(c => c.Id == s_condition_id));
                }

            }
            return list;
        }


        public List<Condition> ListCondition()
        {
            List<Condition> list = new List<Condition>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = @"SELECT s_condition_id, s_condition_name FROM s_condition";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(
                    new Condition
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)//,
                        //Image = reader[2] == null? "": reader.GetString(2)
                    });
                }
            }
            return list;
        }

        public List<PackageType> ListPackageType()
        {
            List<PackageType> list = new List<PackageType>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = @"SELECT PackageTypeId, PackageTypeName FROM s_webpages_PackageTypes";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(
                    new PackageType
                    {
                        PackageTypeId = reader.GetInt32(0),
                        PackageTypeName = reader.GetString(1)
                    });
                }
            }
            return list;
        }

        public List<Category> ListCategory()
        {
            List<Category> list = new List<Category>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = @"SELECT category_id, title, image FROM category";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(
                    new Category
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Image = reader.GetString(2)
                    });
                }
            }
            return list;
        }

        public List<Subcategory> ListSubcategory()
        {
            List<Subcategory> list = new List<Subcategory>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = @"SELECT subcategory_id, title, image FROM subcategory";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(
                    new Subcategory
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1)//,
                        //Image = reader.GetString(2)
                    });
                }
            }
            return list.OrderBy(s=>s.Title).ToList();
        }

        public List<RequestForm> ListForm()
        {
            List<RequestForm> list = new List<RequestForm>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText =
                @"SELECT request_form_id, title, form_name, class_name
                FROM request_form";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(
                    new RequestForm
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        FormName = reader.GetString(2),
                        ClassName = reader.GetString(3)
                    });
                }
            }
            return list;
        }

        public List<RequestForm> ListForm(int categoryId)
        {
            List<RequestForm> list = new List<RequestForm>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                SqlParameter prmCategoryId = new SqlParameter("@CategoryId", categoryId); command.Parameters.Add(prmCategoryId);

                command.CommandText =
                @"SELECT DISTINCT rf.request_form_id, rf.title, rf.form_name, rf.class_name, csrf.subcategory_id, rf.implemented
                FROM request_form rf 
                INNER JOIN category_subcategory_request_form csrf ON rf.class_name = csrf.class_name
                WHERE csrf.category_id = @CategoryId
                ";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(
                    new RequestForm
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        FormName = reader.GetString(2),
                        ClassName = reader.GetString(3),
                        SubcategoryId = reader.GetInt32(4),
                        Implemented = reader.GetBoolean(5)
                    });
                }
            }
            return list.OrderBy(f=>f.Title).ToList();
        }

        public List<Package> ListPackageForUser(int userId)
        {
            List<Package> list = new List<Package>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                SqlParameter UserId = new SqlParameter("@UserId", userId); command.Parameters.Add(UserId);

                command.CommandText =
                @"SELECT p.[PackageId]
                      ,[PackageTypeId]
                      ,[PackagePrice]
                      ,[PackageTermMonth]
                      ,[PackageName]
                      ,[PackageText]
                      ,[Image]
                  FROM [minebidz_iias_ca_sql].[dbo].[s_webpages_Packages] p 
                  INNER JOIN webpages_UsersPackages up
                  ON p.PackageId = up.PackageId
                  WHERE up.UserId = @UserId
                ";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(
                    new Package
                    {
                        PackageId = reader.GetInt32(0),
                        PackageTypeId = reader.GetInt32(1),
                        PackagePrice = reader.GetDecimal(2),
                        PackageTermMonth = reader.GetInt32(3),
                        PackageName = reader.GetString(4),
                        PackageText = reader.GetString(5)
                    });
                }
            }
            return list;
        }

        public Package GetPackage(int id)
        {
            Package form = new Package();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                SqlParameter pid = new SqlParameter("@pid", id); command.Parameters.Add(pid);

                command.CommandText =
                @"SELECT [PackageId]
                      ,[PackageTypeId]
                      ,[PackagePrice]
                      ,[PackageTermMonth]
                      ,[PackageName]
                      ,[PackageText]
                      ,[Image]
                  FROM [minebidz_iias_ca_sql].[dbo].[s_webpages_Packages] WHERE PackageId = @pid";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    form =
                    new Package
                    {
                         PackageId = reader.GetInt32(0),
                         PackageTypeId = reader.GetInt32(1),
                         PackagePrice = reader.GetDecimal(2),
                         PackageTermMonth = reader.GetInt32(3),
                         PackageName = reader.GetString(4),
                         PackageText = reader.GetString(5)
                    };
                }
            }
            return form;
        }

        public Bid GetBid(int bidId)
        {
           Bid bid = new Bid();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                SqlParameter bid_id = new SqlParameter("@bid_id", bidId); command.Parameters.Add(bid_id);

                command.CommandText =
                @"SELECT [bid_id]
                    ,bid.[UserId]
                    ,bid.[request_info_id]
                    ,[amount]
                    ,bid.[description]
                    ,[reference_number]
                    ,[accepted]
                    ,bid.[document_info]
                    ,bid.[approved]
                    ,company.[contact_id]
                    ,company.[company_name]
                    ,company.[contact_name]
                    ,company.[street_address]
                    ,company.[city]
                    ,company.[state_province_code]
                    ,company.[country_code]
                    ,company.[postal_code]
                    ,company.[phone]
                    ,company.[mobile]
                    ,company.[fax]
                    ,company.[email]
      
                  FROM [minebidz_iias_ca_sql].[dbo].[bid] bid
                  INNER JOIN
                  contact_info company ON company.contact_id = bid.contact_info_id
                  WHERE bid_id = @bid_id";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    bid = new Bid
                    {
                        Id = reader.GetInt32(0),
                        UserId = (int)reader["UserId"],
                        Amount = (decimal)reader["amount"],
                        Description = reader["description"] is DBNull ? String.Empty : (string)reader["description"],
                        EngineeringDesign = reader["document_info"] is DBNull ? String.Empty : (string)reader["document_info"],
                        ReferenceNumber = reader["reference_number"] is DBNull ? String.Empty : (string)reader["reference_number"],
                        Accepted = (bool)reader["accepted"],
                        Approved = (bool)reader["approved"],
                        RequestInfoId = (int)reader["request_info_id"],

                        CompanyInfo = new ContactInfo()
                        {
                            Id = (int)reader["contact_id"],
                            City = reader["city"] is DBNull ? String.Empty : (string)reader["city"],
                            CompanyName = reader["company_name"] is DBNull ? String.Empty : (string)reader["company_name"],
                            ContactName = reader["contact_name"] is DBNull ? String.Empty : (string)reader["contact_name"],
                            CountryCode = reader["country_code"] is DBNull ? String.Empty : (string)reader["country_code"],
                            Email = reader["email"] is DBNull ? String.Empty : (string)reader["email"],
                            Fax = reader["fax"] is DBNull ? String.Empty : (string)reader["fax"],
                            Mobile = reader["mobile"] is DBNull ? String.Empty : (string)reader["mobile"],
                            Phone = reader["phone"] is DBNull ? String.Empty : (string)reader["phone"],
                            PostalCode = reader["postal_code"] is DBNull ? String.Empty : (string)reader["postal_code"],
                            ProvinceStateCode = reader["state_province_code"] is DBNull ? String.Empty : (string)reader["state_province_code"],
                            StreetAddress = reader["street_address"] is DBNull ? String.Empty : (string)reader["street_address"]
                        },
                    };
                }
            }
            return bid;
        }


        public RequestForm GetForm(string formId)
        {
            RequestForm form = new RequestForm();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                SqlParameter class_name = new SqlParameter("@class_name", formId); command.Parameters.Add(class_name);

                command.CommandText =
                @"SELECT request_form_id, title, form_name, class_name from request_form WHERE class_name = @class_name";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    form =
                    new RequestForm
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        FormName = reader.GetString(2),
                        ClassName = reader.GetString(3)
                    };
                }
            }
            return form;
        }

        public void ApproveBid(int id, bool approved)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                SqlParameter bid_id = new SqlParameter("@bid_id", id); command.Parameters.Add(bid_id);
                SqlParameter approved_par = new SqlParameter("@approved_par", approved); command.Parameters.Add(approved_par);

                command.CommandText =
                @"UPDATE bid SET  approved = @approved_par WHERE bid_id = @bid_id";
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public void ApproveBidRequest(int requestId, bool approved)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                SqlParameter request_info_id = new SqlParameter("@request_info_id", requestId); command.Parameters.Add(request_info_id);
                SqlParameter approved_par = new SqlParameter("@approved_par", approved); command.Parameters.Add(approved_par);

                command.CommandText =
                @"UPDATE request_info SET approved = @approved_par WHERE request_info_id = @request_info_id";
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public void DeleteBid(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                SqlParameter bid_id = new SqlParameter("@bid_id", id); command.Parameters.Add(bid_id);

                command.CommandText =
                @"UPDATE bid SET deleted = 1 WHERE bid_id = @bid_id";
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public void DeleteBidRequest(int requestId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                SqlParameter request_info_id = new SqlParameter("@request_info_id", requestId); command.Parameters.Add(request_info_id);

                command.CommandText =
                @"UPDATE request_info SET  deleted = 1 WHERE request_info_id = @request_info_id";
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public RequestInfo GetRequestInfo(int requestInfoId)
        {
            RequestInfo requestInfo = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                SqlParameter request_info_id = new SqlParameter("@request_info_id", requestInfoId); command.Parameters.Add(request_info_id);


                command.CommandText = @"SELECT [request_info_id]
                      ,[UserId]
                      ,[details_info]
                      ,[approved]
                        ,[class_name]
                        ,[category_id]
                        ,[subcategory_id]
                        ,[description]
                        ,[document_info]
     
                      ,company.[company_name]
                      ,company.[contact_name]
                      ,company.[contact_id]
                      ,company.[street_address]
                      ,company.[city]
                      ,company.[state_province_code]
                      ,company.[country_code]
                      ,company.[postal_code]
                      ,company.[phone]
                      ,company.[mobile]
                      ,company.[fax]
                      ,company.[email]
      
                      ,delivery.[company_name] as d_company_name
                      ,delivery.[contact_name]as d_contact_name
                      ,delivery.[contact_id]as d_contact_id
                      ,delivery.[street_address] as d_street_address
                      ,delivery.[city] as d_city
                      ,delivery.[state_province_code] as d_state_province_code
                      ,delivery.[country_code] as d_country_code
                      ,delivery.[postal_code] as d_postal_code
                      ,delivery.[phone] as d_phone
                      ,delivery.[mobile] as d_mobile
                      ,delivery.[fax] as d_fax
                      ,delivery.[email] as d_email
                      ,bi.[bid_name]
                      ,bi.[bid_info_id]
                      ,bi.[bid_end]
                      ,bi.[bid_start]
                  FROM [minebidz_iias_ca_sql].[dbo].[request_info] ri
                  INNER JOIN
                  contact_info company ON company.contact_id = ri.company_contact_id
                  INNER JOIN
                  contact_info delivery ON delivery.contact_id = ri.delivery_contact_id
                  INNER JOIN 
                  bid_info bi ON bi.bid_info_id = ri.bid_info_id
                    WHERE ri.request_info_id = @request_info_id";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    requestInfo =
                    new RequestInfo
                    {
                        Id = reader.GetInt32(0),
                        UserId = (int)reader["UserId"],
                        CategoryId = (int)reader["category_id"],
                        SubcategoryId = (int)reader["subcategory_id"],
                        DetailsInfoJson = (string)reader["details_info"],
                        Approved = (bool)reader["approved"],
                        Description = reader["description"] is DBNull ? String.Empty : (string)reader["description"],
                        DocumentInfo = reader["document_info"] is DBNull ? String.Empty : (string)reader["document_info"],
                        ClassName = reader["class_name"] is DBNull ? String.Empty : (string)reader["class_name"],

                        BidInfo = new BidInfo()
                        {
                            BidName = reader["bid_name"] is DBNull ? String.Empty : (string)reader["bid_name"],
                            BidEndDate = (DateTime)reader["bid_end"],
                            BidStartDate = (DateTime)reader["bid_start"],
                            Id = (int)reader["bid_info_id"]
                        },
                        CompanyInfo = new ContactInfo()
                        {
                            City = reader["city"] is DBNull ? String.Empty : (string)reader["city"],
                            CompanyName = reader["company_name"] is DBNull ? String.Empty : (string)reader["company_name"],
                            ContactName = reader["contact_name"] is DBNull ? String.Empty : (string)reader["contact_name"],
                            CountryCode = reader["country_code"] is DBNull ? String.Empty : (string)reader["country_code"],
                            Email = reader["email"] is DBNull ? String.Empty : (string)reader["email"],
                            Fax = reader["fax"] is DBNull ? String.Empty : (string)reader["fax"],
                            Mobile = reader["mobile"] is DBNull ? String.Empty : (string)reader["mobile"],
                            Phone = reader["phone"] is DBNull ? String.Empty : (string)reader["phone"],
                            PostalCode = reader["postal_code"] is DBNull ? String.Empty : (string)reader["postal_code"],
                            ProvinceStateCode = reader["state_province_code"] is DBNull ? String.Empty : (string)reader["state_province_code"],
                            StreetAddress = reader["street_address"] is DBNull ? String.Empty : (string)reader["street_address"],
                            Id = (int)reader["contact_id"]
                        },
                        DeliveryInfo = new ContactInfo()
                        {
                            City = reader["d_city"] is DBNull ? String.Empty : (string)reader["d_city"],
                            CompanyName = reader["d_company_name"] is DBNull ? String.Empty : (string)reader["d_company_name"],
                            ContactName = reader["d_contact_name"] is DBNull ? String.Empty : (string)reader["d_contact_name"],
                            CountryCode = reader["d_country_code"] is DBNull ? String.Empty : (string)reader["d_country_code"],
                            Email = reader["d_email"] is DBNull ? String.Empty : (string)reader["d_email"],
                            Fax = reader["d_fax"] is DBNull ? String.Empty : (string)reader["d_fax"],
                            Mobile = reader["d_mobile"] is DBNull ? String.Empty : (string)reader["d_mobile"],
                            Phone = reader["d_phone"] is DBNull ? String.Empty : (string)reader["d_phone"],
                            PostalCode = reader["d_postal_code"] is DBNull ? String.Empty : (string)reader["d_postal_code"],
                            ProvinceStateCode = reader["d_state_province_code"] is DBNull ? String.Empty : (string)reader["d_state_province_code"],
                            StreetAddress = reader["d_street_address"] is DBNull ? String.Empty : (string)reader["d_street_address"],
                            Id = (int)reader["d_contact_id"]
                        },
                         ConditionList = new List<Condition>(),
                    };
                }

                command.CommandText = @"SELECT [s_condition_name]
                          ,ci.[s_condition_id]
                      FROM [minebidz_iias_ca_sql].[dbo].[condition_info] ci
                    INNER JOIN s_condition sc ON sc.s_condition_id = ci.s_condition_id
                    WHERE ci.request_info_id = @request_info_id";

                reader.Close();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string s_condition_name = reader.GetString(0);
                    int s_condition_id = reader.GetInt32(1);
                    requestInfo.ConditionList.Add(new Condition() { Id = s_condition_id, Name = s_condition_name });
                }
            }
            return requestInfo;
        }

        public void SaveBid(Bid bid)
        {
            int contactId;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction("Bid");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    contactId = SaveContact(command, bid.CompanyInfo);
                    command.Parameters.Clear();

                    SqlParameter UserId = new SqlParameter("@UserId", bid.UserId); command.Parameters.Add(UserId);
                    SqlParameter contact_info_id = new SqlParameter("@contact_info_id", contactId); command.Parameters.Add(contact_info_id);
                    SqlParameter request_info_id = new SqlParameter("@request_info_id", bid.RequestInfoId); command.Parameters.Add(request_info_id);
                    SqlParameter amount = new SqlParameter("@amount", bid.Amount); command.Parameters.Add(amount);
                    SqlParameter description = new SqlParameter("@description", (object)bid.Description ?? DBNull.Value); command.Parameters.Add(description);
                    SqlParameter accepted = new SqlParameter("@accepted", false); command.Parameters.Add(accepted);
                    SqlParameter reference_number = new SqlParameter("@reference_number", bid.ReferenceNumber); command.Parameters.Add(reference_number);
                    SqlParameter document_info = new SqlParameter("@document_info", (object)bid.EngineeringDesign ?? DBNull.Value); command.Parameters.Add(document_info);


                    command.CommandText =
                    @"INSERT INTO [minebidz_iias_ca_sql].[dbo].[bid]
                           ([UserId]
                           ,[contact_info_id]
                           ,[request_info_id]
                           ,[amount]
                           ,[description]
                           ,[reference_number]
                            ,[accepted]
                    ,[document_info])
                     VALUES
                           (@UserId
                           ,@contact_info_id
                           ,@request_info_id
                           ,@amount
                           ,@description
                           ,@reference_number
                            ,@accepted
                        ,@document_info)";

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Attempt to roll back the transaction. 
                    try
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    catch (Exception ex2)
                    {
                        //Console.WriteLine("  Message: {0}", ex2.Message);
                        throw ex2;
                    }
                }
            }
        }

        public void UpdateBid(Bid bid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction("BidUpdate");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    UpdateContact(command, bid.CompanyInfo);

                    command.Parameters.Clear();

                    SqlParameter description = new SqlParameter("@description", (object)bid.Description ?? DBNull.Value); command.Parameters.Add(description);
                    SqlParameter reference_number = new SqlParameter("@reference_number", bid.ReferenceNumber); command.Parameters.Add(reference_number);
                    SqlParameter bid_id = new SqlParameter("@bid_id", bid.Id); command.Parameters.Add(bid_id);

                    command.CommandText =
                    @"UPDATE [minebidz_iias_ca_sql].[dbo].[bid]
                        SET [description] = @description
                           ,[reference_number] = @reference_number
                        WHERE [bid_id] = @bid_id";

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Attempt to roll back the transaction. 
                    try
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    catch (Exception ex2)
                    {
                        //Console.WriteLine("  Message: {0}", ex2.Message);
                        throw ex2;
                    }
                }
            }
        }

        private void UpdateContact(SqlCommand command, ContactInfo contactInfo)
        {
            SqlParameter company_name = new SqlParameter("@company_name", (object)contactInfo.CompanyName ?? DBNull.Value); command.Parameters.Add(company_name);
            SqlParameter contact_name = new SqlParameter("@contact_name", (object)contactInfo.ContactName ?? DBNull.Value); command.Parameters.Add(contact_name);
            SqlParameter street_address = new SqlParameter("@street_address", (object)contactInfo.StreetAddress ?? DBNull.Value); command.Parameters.Add(street_address);
            SqlParameter city = new SqlParameter("@city", (object)contactInfo.City ?? DBNull.Value); command.Parameters.Add(city);
            SqlParameter state_province_code = new SqlParameter("@state_province_code", (object)contactInfo.ProvinceStateCode ?? DBNull.Value); command.Parameters.Add(state_province_code);
            SqlParameter country_code = new SqlParameter("@country_code", (object)contactInfo.CountryCode ?? DBNull.Value); command.Parameters.Add(country_code);
            SqlParameter postal_code = new SqlParameter("@postal_code", (object)contactInfo.PostalCode ?? DBNull.Value); command.Parameters.Add(postal_code);
            SqlParameter phone = new SqlParameter("@phone", (object)contactInfo.Phone ?? DBNull.Value); command.Parameters.Add(phone);
            SqlParameter mobile = new SqlParameter("@mobile", (object)contactInfo.Mobile ?? DBNull.Value); command.Parameters.Add(mobile);
            SqlParameter fax = new SqlParameter("@fax", (object)contactInfo.Fax ?? DBNull.Value); command.Parameters.Add(fax);
            SqlParameter email = new SqlParameter("@email", (object)contactInfo.Email ?? DBNull.Value); command.Parameters.Add(email);
            SqlParameter contact_id = new SqlParameter("@contact_id", contactInfo.Id); command.Parameters.Add(contact_id);

            command.CommandText =
            @"UPDATE [minebidz_iias_ca_sql].[dbo].[contact_info]
                SET
                   [company_name] = @company_name
                   ,[contact_name] = @contact_name
                   ,[street_address] = @street_address
                   ,[city] = @city 
                   ,[state_province_code] = @state_province_code
                   ,[country_code] = @country_code
                   ,[postal_code] = @postal_code
                   ,[phone] = @phone 
                   ,[mobile] = @mobile
                   ,[fax] = @fax
                   ,[email] = @email
                    WHERE contact_id = @contact_id ";

            command.ExecuteNonQuery();
        }


        public int SaveForm(RequestInfo request)
        {
            int companyContactId;
            int deliveryContactId;
            int bidInfoId;
            int requestId;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction("Request");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    companyContactId = SaveContact(command, request.CompanyInfo);
                    command.Parameters.Clear();

                    deliveryContactId = SaveContact(command, request.DeliveryInfo);
                    command.Parameters.Clear();

                    SqlParameter bid_name = new SqlParameter("@bid_name", (object)request.BidInfo.BidName ?? DBNull.Value); command.Parameters.Add(bid_name);
                    SqlParameter bid_end = new SqlParameter("@bid_end", request.BidInfo.BidEndDate); command.Parameters.Add(bid_end);
                    SqlParameter bid_start = new SqlParameter("@bid_start", request.BidInfo.BidStartDate); command.Parameters.Add(bid_start);


                    command.CommandText =
                    @"INSERT INTO [minebidz_iias_ca_sql].[dbo].[bid_info]
                           ([bid_name]
                           ,[bid_end]
                            ,[bid_start])
                     VALUES
                           (@bid_name
                           ,@bid_end 
                            ,@bid_start );
                    SELECT CAST(@@IDENTITY AS INT) ";

                    bidInfoId = (int)command.ExecuteScalar();
                    command.Parameters.Clear();

                    SqlParameter UserId = new SqlParameter("@UserId", (object)request.UserId ?? DBNull.Value); command.Parameters.Add(UserId);
                    SqlParameter company_contact_id = new SqlParameter("@company_contact_id", companyContactId); command.Parameters.Add(company_contact_id);
                    SqlParameter delivery_contact_id = new SqlParameter("@delivery_contact_id", deliveryContactId); command.Parameters.Add(delivery_contact_id);
                    SqlParameter bid_info_id = new SqlParameter("@bid_info_id", bidInfoId); command.Parameters.Add(bid_info_id);
                    SqlParameter details_info = new SqlParameter("@details_info", request.DetailsInfoJson); command.Parameters.Add(details_info);
                    SqlParameter approved = new SqlParameter("@approved", request.Approved); command.Parameters.Add(approved);
                    SqlParameter class_name = new SqlParameter("@class_name", request.ClassName); command.Parameters.Add(class_name);
                    SqlParameter category_id = new SqlParameter("@category_id", request.CategoryId); command.Parameters.Add(category_id);
                    SqlParameter subcategory_id = new SqlParameter("@subcategory_id", request.SubcategoryId); command.Parameters.Add(subcategory_id);
                    SqlParameter description = new SqlParameter("@description", (object)request.Description ?? DBNull.Value); command.Parameters.Add(description);
                    SqlParameter document_info = new SqlParameter("@document_info", (object)request.DocumentInfo ?? DBNull.Value); command.Parameters.Add(document_info);


                    command.CommandText =
                    @"INSERT INTO [minebidz_iias_ca_sql].[dbo].[request_info]
                           ([UserId]
                           ,[company_contact_id]
                           ,[delivery_contact_id]
                           ,[bid_info_id]
                           ,[details_info]
                           ,[approved]
                            ,[class_name]
                            ,[category_id]
                            ,[subcategory_id]
                            ,[description]
                            ,[document_info])
                     VALUES
                           (@UserId
                           ,@company_contact_id 
                           ,@delivery_contact_id 
                           ,@bid_info_id 
                           ,@details_info 
                           ,@approved
                           ,@class_name 
                           ,@category_id 
                           ,@subcategory_id
                            ,@description
                            ,@document_info);
                    SELECT CAST(@@IDENTITY AS INT) ";

                    requestId = (int)command.ExecuteScalar();

                    if (request.ConditionList != null && request.ConditionList.Count > 0)
                    {
                        foreach(Condition condition in request.ConditionList)
                        {
                            command.Parameters.Clear();
                            SqlParameter request_info_id = new SqlParameter("@request_info_id", requestId); command.Parameters.Add(request_info_id);
                            SqlParameter s_condition_id = new SqlParameter("@s_condition_id", condition.Id); command.Parameters.Add(s_condition_id);
                            command.CommandText =
                            @"INSERT INTO [minebidz_iias_ca_sql].[dbo].[condition_info]
                                   ([request_info_id]
                                   ,[s_condition_id])
                             VALUES
                                   (@request_info_id
                                   ,@s_condition_id)";

                            command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Attempt to roll back the transaction. 
                    try
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    catch (Exception ex2)
                    {
                        //Console.WriteLine("  Message: {0}", ex2.Message);
                        throw ex2;
                    }
                }

                return requestId;
            }
        }

        public IEnumerable<Country> ListCountries()
        {
            List<Country> list = new List<Country>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = @"SELECT country_name, country_code FROM s_country";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(
                    new Country
                    {
                         CountryName = reader.GetString(0),
                          CountryCode = reader.GetString(1)
                    });
                }
            }

            list = list.OrderBy(c => c.CountryName).ToList();
            list.Insert(0, new Country { CountryCode = "US", CountryName = "United States" });
            list.Insert(1, new Country { CountryCode = "CA", CountryName = "Canada" });

            return list;
        }

        public IEnumerable<ProvinceState> ListProvincesStates(string countryCode)
        {
            return ListProvincesStates().Where(p => p.CountryCode == countryCode).OrderBy(c => c.ProvinceStateName).ToList();
        }

        public IEnumerable<ProvinceState> ListProvincesStates()
        {
            List<ProvinceState> list = new List<ProvinceState>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = @"SELECT province_name, province_code, country_code FROM s_province";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(
                    new ProvinceState
                    {
                        ProvinceStateName = reader.GetString(0),
                        ProvinceStateCode = reader.GetString(1),
                        CountryCode = reader.GetString(2)
                    });
                }
            }

            return list.OrderBy(c => c.ProvinceStateName).ToList();
        }

        public IEnumerable<string> ListPumpType()
        {
            return new List<string>()
            {
                "",
                "SRL Centrifugal Slurry Pump",
                "Plunger Pump",
                "Self-Priming Centrifugal",
                "Vertical Slurry Pump",
                "Submersible Pump",
                "Chemical Pump",
                "Horizontal Split-Case Pump",
                "Dry Pit Pumps",
                "Diaphram Pump",
                "End Suction",
                "Fire Pump",
                "Other"
            };
        }

        public IEnumerable<string> ListTankStyleUnit()
        {
            return new List<string>()
            {
                "",
                "Cascading",
                "Carrousel",
                "Other"
            };
        }


         public IEnumerable<string> ListTubingType()
        {
            return new List<string>()
            {
                "",
                "Layflat",
                "Reinforced",
                "Oval",
                "Spiral Reinforced Oval",
                "Shaft",
                "Steel Round",
                "Steel Oval",
                "Other"
            };
        }

        public IEnumerable<string> ListLengthMetersFeet()
        {
            return new List<string>()
            {
                "",
                "Meters",
                "Feet",
                "Other"
            };
        }

        public IEnumerable<string> ListLengthMetersInches()
        {
            return new List<string>()
            {
                "",
                "Meters",
                "Inches",
                "Other"
            };
        }

        public IEnumerable<string> ListLinerType()
        {
            return new List<string>()
            {
                "",
                "Steel",
                "Rubber",
                "Other"
            };
        }


        public IEnumerable<string> ListUnitDiameter()
        {
            return new List<string>()
            {
                "",
                "Inch",
                "mm",
                "Feet",
                "Meter",
                "Other"
            };
        }

        public IEnumerable<string> ListCouplingsUnit()
        {
            return new List<string>()
            {
                "",
                "Snap-Roc",
                "Ring",
                "Zipper",
                "Steel",
                "Other"
            };
        }

        public IEnumerable<string> ListVentAccessories()
        {
            return new List<string>()
            {
                "",
                "Inflatable Head Cover",
                "Inflatable Stopping Panel",
                "Messenger Cable",
                "Wye + Tee Fittings",
                "Engineered Elbows",
                "Other"
            };
        }

        public IEnumerable<string> ListProcessType()
        {
            return new List<string>()
            {
                "",
                "Gravity",
                "Flotation",
                "ADR",
                "Merril-Crowe",
                "Carbon in Leach",
                "Carbon in Pulp",
                "Other"
            };
        }

        public IEnumerable<string> ListOutletType()
        {
            return new List<string>()
            {
                "",
                "Overflow",
                "Grate",
                "Other"
            };
        }


        public IEnumerable<string> ListMillProcessType()
        {
            return new List<string>()
            {
                "",
                "Wet",
                "Dry"
            };
        }


        public IEnumerable<string> ListCrusherType()
        {
            return new List<string>()
            {
                "",
                "GYRATORY",
                "JAW",
                "CONE/STANDARD",
                "CONE/SHORT HEAD",
                "ROLL",
                "IMPACT",
                "OTHER"
            }.OrderBy(r => r).ToList();
        }


        public IEnumerable<string> ListProductTargetUnit()
        {
            return new List<string>()
            {
                "",
                "TPH",
                "MTPH",
            };
        }


        public IEnumerable<string> ListDewateringType()
        {
            return new List<string>()
            {
                "",
                "Thickener",
                "Cyclone",
                "Plate Frame",
                "Rotary Drying Kiln",
                "Leaf",
                "Horizontal",
                "Drum",
                "Disc",
                "Other"
            };
        }

        public IEnumerable<string> ListCircuitType()
        {
            return new List<string>()
            {
                "",
                "OPEN",
                "CLOSED"
            };
        }


        public IEnumerable<string> ListCarbonColumnSolutionFeedGradeUnit()
        {
            return new List<string>()
            {
                "",
                "Oz/T",
                "g/t",
                "ppm"
            };
        }

        public IEnumerable<string> ListMillType()
        {
            return new List<string>()
            {
                "",
                "SAG/AG",
                "Rod",
                "Ball",
                "Pebble",
                "Verti/Tower",
                "Other"
            };
        }

        public IEnumerable<string> ListMotionType()
        {
            return new List<string>()
            {
                "",
                "Vibrating",
                "Shaking",
                "Reciprocating",
                "Gyratory",
                "Rotating",
                "Other"
            };
        }

        public IEnumerable<string> ListDescType()
        {
            return new List<string>()
            {
                "",
                "Single",
                "Double",
                "Triple",
                "Other"
            };
        }

        public IEnumerable<string> ListRecommendedPower()
        {
            return new List<string>()
            {
                "",
                "kW",
                "hp",
                "Other"
            };
        }

        public IEnumerable<string> ListExcavatorType()
        {
            return new List<string>()
            {
                "",
                "Backhoe",
                "Shovel",
                "Truckhoe",
                "Other"
            };
        }

        public IEnumerable<string> ListSizeClassExcavatorMetricTon()
        {
            return new List<string>()
            {
                "",
                "20-25",
                "30-35",
                "40-50",
                "63",
                "70",
                "Other"
            };
        }

        public IEnumerable<string> ListFilterUnit()
        {
            return new List<string>()
            {
                "",
                "Each",
                "Case",
                "Pallet",
                "Bulk",
                "Drums",
                "Barrels",
                "Super Sack",
                "Bag",
                "Bundles",
                "Other"
            };
        }

        public IEnumerable<string> ListClassifierType()
        {
            return new List<string>()
            {
                "",
                "Hydrocyclone",
                "Rake",
                "Spiral",
                "Other"
            };
        }

        public IEnumerable<string> ListMaterial()
        {
            return new List<string>()
            {
                "",
                "Other",
                "Cast Iron",
                "Stainless Steel",
                "Bronze"
            };
        }

        public void SaveUserInfo(ContactInfo contactInfo, int userId, int? paymentTypeId, int? logoPackageId, int? biddingPackageId)
        {
            int companyContactId;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction("Request");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    companyContactId = SaveContact(command, contactInfo);
                    command.Parameters.Clear();

                    SqlParameter UserId = new SqlParameter("@UserId", userId); command.Parameters.Add(UserId);
                    SqlParameter contact_info_id = new SqlParameter("@contact_info_id", companyContactId); UserId.IsNullable = true; command.Parameters.Add(contact_info_id);

                    command.CommandText =
                    @"UPDATE [minebidz_iias_ca_sql].[dbo].[UserProfile]
                       SET [contact_info_id] = @contact_info_id 
                     WHERE UserId = @UserId ";

                    command.ExecuteNonQuery();
                    command.Parameters.Clear();

                    SqlParameter PaymentTypeId = new SqlParameter("@PaymentTypeId", paymentTypeId ?? 0); 
                    SqlParameter Approved = new SqlParameter("@Approved", false); 

                    if (logoPackageId.HasValue)
                    {
                        SqlParameter PackageId = new SqlParameter("@PackageId", logoPackageId.Value); command.Parameters.Add(PackageId);
                        command.Parameters.Add(UserId); 
                        command.Parameters.Add(PaymentTypeId); 
                        command.Parameters.Add(Approved);

                        command.CommandText =
                        @"INSERT INTO [minebidz_iias_ca_sql].[dbo].[webpages_UsersPackages]
                           ([UserId]
                           ,[PackageId]
                           ,[Approved]
                           ,[PaymentTypeId])
                     VALUES
                           (@UserId
                           ,@PackageId
                           ,@Approved
                           ,@PaymentTypeId) ";

                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }

                    if (biddingPackageId.HasValue)
                    {
                        SqlParameter PackageId = new SqlParameter("@PackageId", biddingPackageId.Value); command.Parameters.Add(PackageId);
                        command.Parameters.Add(UserId);
                        command.Parameters.Add(PaymentTypeId);
                        command.Parameters.Add(Approved);

                        command.CommandText =
                        @"INSERT INTO [minebidz_iias_ca_sql].[dbo].[webpages_UsersPackages]
                           ([UserId]
                           ,[PackageId]
                           ,[Approved]
                           ,[PaymentTypeId])
                     VALUES
                           (@UserId
                           ,@PackageId
                           ,@Approved
                           ,@PaymentTypeId) ";

                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Attempt to roll back the transaction. 
                    try
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    catch (Exception ex2)
                    {
                        //Console.WriteLine("  Message: {0}", ex2.Message);
                        throw ex2;
                    }
                }
            }
        }

        private int SaveContact(SqlCommand command, ContactInfo contactInfo)
        {
            SqlParameter company_name = new SqlParameter("@company_name", (object)contactInfo.CompanyName ?? DBNull.Value); command.Parameters.Add(company_name);
            SqlParameter contact_name = new SqlParameter("@contact_name", (object)contactInfo.ContactName ?? DBNull.Value); command.Parameters.Add(contact_name);
            SqlParameter street_address = new SqlParameter("@street_address", (object)contactInfo.StreetAddress ?? DBNull.Value); command.Parameters.Add(street_address);
            SqlParameter city = new SqlParameter("@city", (object)contactInfo.City ?? DBNull.Value); command.Parameters.Add(city);
            SqlParameter state_province_code = new SqlParameter("@state_province_code", (object)contactInfo.ProvinceStateCode ?? DBNull.Value); command.Parameters.Add(state_province_code);
            SqlParameter country_code = new SqlParameter("@country_code", (object)contactInfo.CountryCode ?? DBNull.Value); command.Parameters.Add(country_code);
            SqlParameter postal_code = new SqlParameter("@postal_code", (object)contactInfo.PostalCode ?? DBNull.Value); command.Parameters.Add(postal_code);
            SqlParameter phone = new SqlParameter("@phone", (object)contactInfo.Phone ?? DBNull.Value); command.Parameters.Add(phone);
            SqlParameter mobile = new SqlParameter("@mobile", (object)contactInfo.Mobile ?? DBNull.Value); command.Parameters.Add(mobile);
            SqlParameter fax = new SqlParameter("@fax", (object)contactInfo.Fax ?? DBNull.Value); command.Parameters.Add(fax);
            SqlParameter email = new SqlParameter("@email", (object)contactInfo.Email ?? DBNull.Value); command.Parameters.Add(email);

            command.CommandText =
            @"INSERT INTO [minebidz_iias_ca_sql].[dbo].[contact_info]
                   ([company_name]
                   ,[contact_name]
                   ,[street_address]
                   ,[city]
                   ,[state_province_code]
                   ,[country_code]
                   ,[postal_code]
                   ,[phone]
                   ,[mobile]
                   ,[fax]
                   ,[email])
             VALUES
                   (@company_name
                   ,@contact_name
                   ,@street_address
                   ,@city 
                   ,@state_province_code
                   ,@country_code
                   ,@postal_code 
                   ,@phone 
                   ,@mobile 
                   ,@fax 
                   ,@email);
                    SELECT CAST(@@IDENTITY AS INT) ";

            return (int)command.ExecuteScalar();
        }

        public bool UserCanBid(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                SqlParameter UserId = new SqlParameter("@UserId", userId); command.Parameters.Add(UserId);

                command.CommandText =
                @"SELECT TOP 1 * FROM 
                [minebidz_iias_ca_sql].[dbo].[webpages_UsersPackages] up
                INNER JOIN [minebidz_iias_ca_sql].[dbo].[s_webpages_Packages] p
                ON up.PackageId = p.PackageId
                WHERE GETDATE() BETWEEN up.StartDate AND up.EndDate
                AND p.PackageTypeId = 2 AND up.Approved = 1
                AND UserId = @UserId ";
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void UpdateForm(RequestInfo request)
        {
            RequestInfo rInfo = GetRequestInfo(request.Id);

            request.CompanyInfo.Id = rInfo.CompanyInfo.Id;
            request.DeliveryInfo.Id = rInfo.DeliveryInfo.Id;
            request.BidInfo.Id = rInfo.BidInfo.Id;
            int requestId = request.Id;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction("Request");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    UpdateContact(command, request.CompanyInfo);
                    command.Parameters.Clear();

                    UpdateContact(command, request.DeliveryInfo);
                    command.Parameters.Clear();

                    SqlParameter bid_name = new SqlParameter("@bid_name", (object)request.BidInfo.BidName ?? DBNull.Value); command.Parameters.Add(bid_name);
                    SqlParameter bid_end = new SqlParameter("@bid_end", request.BidInfo.BidEndDate); command.Parameters.Add(bid_end);
                    SqlParameter bid_start = new SqlParameter("@bid_start", request.BidInfo.BidStartDate); command.Parameters.Add(bid_start);
                    SqlParameter bid_info_id = new SqlParameter("@bid_info_id", request.BidInfo.Id); command.Parameters.Add(bid_info_id);


                    command.CommandText =
                    @"UPDATE [minebidz_iias_ca_sql].[dbo].[bid_info]
                       SET [bid_name] = @bid_name
                           ,[bid_end] = @bid_end
                            ,[bid_start] = @bid_start
                    WHERE bid_info_id = @bid_info_id ";

                    command.ExecuteNonQuery();
                    command.Parameters.Clear();

                    if (String.IsNullOrEmpty(request.DocumentInfo))
                    {
                        request.DocumentInfo = rInfo.DocumentInfo;
                    }

                    SqlParameter details_info = new SqlParameter("@details_info", request.DetailsInfoJson); command.Parameters.Add(details_info);
                    SqlParameter description = new SqlParameter("@description", (object)request.Description ?? DBNull.Value); command.Parameters.Add(description);
                    SqlParameter document_info = new SqlParameter("@document_info", (object)request.DocumentInfo ?? DBNull.Value); command.Parameters.Add(document_info);
                    SqlParameter request_info_id = new SqlParameter("@request_info_id", request.Id); command.Parameters.Add(request_info_id);


                    command.CommandText =
                    @"UPDATE [minebidz_iias_ca_sql].[dbo].[request_info]
                         SET  [details_info] = @details_info
                            ,[description] = @description
                            ,[document_info] = @document_info
                    WHERE request_info_id = @request_info_id ";

                    command.ExecuteNonQuery();
                    command.Parameters.Clear();

                    command.Parameters.Add(request_info_id);
                    command.CommandText =
                            @"DELETE FROM [minebidz_iias_ca_sql].[dbo].[condition_info]
                              WHERE request_info_id = @request_info_id";

                    command.ExecuteNonQuery();

                    if (request.ConditionList != null && request.ConditionList.Count > 0)
                    {
                        foreach (Condition condition in request.ConditionList)
                        {
                            command.Parameters.Clear();
                            command.Parameters.Add(request_info_id);
                            SqlParameter s_condition_id = new SqlParameter("@s_condition_id", condition.Id); command.Parameters.Add(s_condition_id);
                            command.CommandText =
                            @"INSERT INTO [minebidz_iias_ca_sql].[dbo].[condition_info]
                                   ([request_info_id]
                                   ,[s_condition_id])
                             VALUES
                                   (@request_info_id
                                   ,@s_condition_id)";

                            command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Attempt to roll back the transaction. 
                    try
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    catch (Exception ex2)
                    {
                        //Console.WriteLine("  Message: {0}", ex2.Message);
                        throw ex2;
                    }
                }
            }
        }
    }
}
