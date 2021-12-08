using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Data; 

public class CatalogContextSeed
{
    public static async Task SeedAsync(CatalogContext catalogContext,
        ILogger logger,
        int retry = 0)
    {
        var retryForAvailability = retry;
        try
        {
            if (catalogContext.Database.IsSqlServer())
            {
                catalogContext.Database.Migrate();
            }

            if (!await catalogContext.CatalogBrands.AnyAsync())
            {
                await catalogContext.CatalogBrands.AddRangeAsync(
                    GetPreconfiguredCatalogBrands());

                await catalogContext.SaveChangesAsync();
            }

            if (!await catalogContext.CatalogTypes.AnyAsync())
            {
                await catalogContext.CatalogTypes.AddRangeAsync(
                    GetPreconfiguredCatalogTypes());

                await catalogContext.SaveChangesAsync();
            }

            if (!await catalogContext.CatalogItems.AnyAsync())
            {
                await catalogContext.CatalogItems.AddRangeAsync(
                    GetPreconfiguredItems());

                await catalogContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            if (retryForAvailability >= 10) throw;

            retryForAvailability++;

            logger.LogError(ex.Message);
            await SeedAsync(catalogContext, logger, retryForAvailability);
            throw;
        }
    }

    static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
    {
        return new List<CatalogBrand>
            {
                new("Champion"),
                new("Legacy"),
                new("League"),
                new("Nordic"),
                new("CI sport"),
                new("Other")
            };
    }

    static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
    {
        return new List<CatalogType>
            {
                new("Hoods"),
                new("Caps"),
                new("Drinkware"),
                new("Mugs"),
                new("Face Masks")
            };
    }

    static IEnumerable<CatalogItem> GetPreconfiguredItems()
    {
        return new List<CatalogItem>
            {
                new(3,5, "LEAGUE FACEMASK - SWEATER STYLE", "LEAGUE FACEMASK - SWEATER STYLE", 3.25,  "https://shop.css.edu/collections/face-masks/products/league-facemask-sweater-style"),
                new(3,5, "LEAGUE HEATHER CHAMP FACE COVERING - GREY WITH NAVY SHIELDS", "LEAGUE HEATHER CHAMP FACE COVERING - GREY WITH NAVY SHIELDS", 3.99, "https://shop.css.edu/collections/face-masks/products/league-heather-champ-face-covering-grey-with-navy-shields"),
                new(1,1, "CHAMPION REVERSE WEAVE HOOD - OATMEAL", "CHAMPION REVERSE WEAVE HOOD - OATMEAL", 59.99,  "https://shop.css.edu/collections/hoods/products/champion-reverse-weave-hood-oatmeal"),
                new(5,1, "CI SPORT CSS PULLOVER HOOD - CHARCOAL HEATHER", "CI SPORT CSS PULLOVER HOOD - CHARCOAL HEATHER", 49.99, "https://shop.css.edu/collections/hoods/products/ci-sport-css-pullover-hood-charcoal-heather"),
                new(2,2, "LEGACY DTA - NAVY TRUCKER WITH MESH BACK", "LEGACY DTA - NAVY TRUCKER WITH MESH BACK", 24.99, "https://shop.css.edu/collections/caps/products/legacy-navy-trucker-with-mesh-back"),
                new(1,2, "CHAMPION TWILL UNSTRUCTED CAP - WHITE", "CHAMPION TWILL UNSTRUCTED CAP - WHITE", 19.99, "https://shop.css.edu/collections/caps/products/champion-twill-unstructed-cap-white"),
                new(4,3, "NORDIC WIDE MOUTH WATER BOTTLE - NAVY", "NORDIC WIDE MOUTH WATER BOTTLE - NAVY",  14.99, "https://shop.css.edu/collections/drinkware/products/nordic-wide-mouth-water-bottle-navy"),
                new(6,4, "HUDSON TRAVEL MUG - GRAPHITE", "HUDSON TRAVEL MUG - GRAPHITE", 7.99, "https://shop.css.edu/collections/mugs-1/products/hudson-travel-mug-graphite"),
                new(6,1, "UNDER ARMOUR AMOUR FLEECE PULLOVER HOOD - TRUE GREY HEATHER", "CUNDER ARMOUR AMOUR FLEECE PULLOVER HOOD - TRUE GREY HEATHER", 61.99, "https://shop.css.edu/collections/hoods/products/under-armour-amour-fleece-pullover-hood-true-grey-heather"),
            };
    }
}
}
