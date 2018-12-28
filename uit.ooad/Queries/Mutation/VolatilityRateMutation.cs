using GraphQL.Types;
using uit.ooad.Businesses;
using uit.ooad.Models;
using uit.ooad.ObjectTypes;
using uit.ooad.Queries.Base;

namespace uit.ooad.Queries.Mutation
{
    public class VolatilityRateMutation : QueryType<VolatilityRate>
    {
        public VolatilityRateMutation()
        {
            Field<NonNullGraphType<VolatilityRateType>>(
                _Creation,
                "Tạo và trả về một giá biến động mới",
                _InputArgument<VolatilityRateCreateInput>(),
                _CheckPermission_TaskObject(
                    p => p.PermissionManageRate,
                    context => VolatilityRateBusiness.Add(_GetInput(context))
                )
            );
        }
    }
}
