using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Tags
{
    public class ListQuery : IRequest<TagsEnvelopeDTO>
    {
    }
}
