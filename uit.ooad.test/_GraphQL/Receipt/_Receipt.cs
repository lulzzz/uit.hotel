using Microsoft.VisualStudio.TestTools.UnitTesting;
using uit.ooad.Businesses;
using uit.ooad.DataAccesses;
using uit.ooad.Models;
using uit.ooad.Queries.Helper;
using uit.ooad.test.Helper;

namespace uit.ooad.test._GraphQL
{
    [TestClass]
    public class _Receipt : RealmDatabase
    {
        [TestMethod]
        public void Mutation_CreateReceipt()
        {
            SchemaHelper.Execute(
                @"/_GraphQL/Receipt/mutation.createReceipt.gql",
                @"/_GraphQL/Receipt/mutation.createReceipt.schema.json",
                new
                {
                    input = new
                    {
                        money = 1000,
                        bankAccountNumber = "10-10-2015",
                        bill = new
                        {
                            id = 1
                        }
                    }
                },
                p => p.PermissionManageHiringRoom = true
            );
        }

        [TestMethod]
        public void Query_Receipt()
        {
            Database.WriteAsync(realm => realm.Add(new Receipt
            {
                Id = 10,
                Money = 1,
                BankAccountNumber = "11111",
                Bill = BillBusiness.Get(1),
                Employee = EmployeeBusiness.Get(Constant.adminName)
            })).Wait();
            SchemaHelper.Execute(
                @"/_GraphQL/Receipt/query.receipt.gql",
                @"/_GraphQL/Receipt/query.receipt.schema.json",
                new { id = 10 },
                p => p.PermissionGetAccountingVoucher = true
            );
        }

        [TestMethod]
        public void Query_Receipts()
        {
            SchemaHelper.Execute(
                @"/_GraphQL/Receipt/query.receipts.gql",
                @"/_GraphQL/Receipt/query.receipts.schema.json",
                null,
                p => p.PermissionGetAccountingVoucher = true
            );
        }
    }
}
