export class Brand
{
    public id:Number|undefined;
    public name:String |undefined;
}

export class ProductGroup
{
    public id: Number | undefined;
    public name: String | undefined;
    public image: String | undefined;
}

export class Price
{
    public productID: Number  | undefined;
    public basePrice: Number | undefined
    public shopName: String | undefined
}

export class Review
{
    public productID : Number | undefined;
    public author: String | undefined;
    public text:String | undefined;
    public score:Number | undefined;
}
export class Specification
{
    public key:String | undefined;
    public value:String | undefined;
}
export class Product
{
    public id:Number | undefined;
    public brand:Brand | undefined;
    public name:String | undefined;
    public productGroup:ProductGroup | undefined;
    public score: Number | undefined;
    public image:String | undefined;   
}