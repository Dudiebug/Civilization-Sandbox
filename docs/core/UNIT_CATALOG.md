# Authoritative Unit Catalog

These semantic units are mandatory now. Exact fixed-point ranges, scales, and serialized encodings are accepted by the first owning implementation task or a measured ADR; TASK-002 does not invent them. D03 still owns the later Version 1.0 scale envelope.

| Domain | Unit/type rule |
|---|---|
| World time | `WorldTime`: signed 64-bit simulation seconds from epoch |
| Duration | Typed duration; never raw wall/frame delta in domain logic |
| Position/distance | Explicit world meters and local coordinates; no unitless vectors across APIs |
| Area | Square meters/hectares as declared by field |
| Quantity/inventory | Integer commodity base units with content-defined conversion |
| Money | Signed 64-bit minor units per currency/entitlement system |
| Rates | Rational or fixed-point per typed duration |
| Utility inputs | Canonical normalized fixed range with named curve and source unit |
| Probability/randomness | Integer/fixed thresholds from keyed streams; no uncontrolled floats |
| Health/need/attitude | Bounded typed values with safe threshold, trend, cadence, and source |
| Capacity/throughput | Quantity per typed time with route/building/equipment scope |
| Confidence | Bounded value plus observation time and source |

Every serialized field and content definition declares units. Conversions are centralized and tested. A placeholder package or schema field may reserve the semantic unit without choosing an unapproved numeric range.
