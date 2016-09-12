using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Mvc {
    public class UnprocessableEntityResult : ObjectResult {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.AspNetCore.Mvc.OkObjectResult" /> class.
        /// </summary>
        /// <param name="value">The content to format into the entity body.</param>
        public UnprocessableEntityResult(object value)
          : base(value) {
            this.StatusCode = new int?(422);
        }
    }
}
