export interface UserProfile{
    email?: string;
    username?: string;
    memberProfile?: {
        age?: number,
        gender?: string;
        countryIso?:{
            name?: string;
            twoLetterCode?: string;
            threeLetterCode?: string;
            numericCode?: string;
        },
        memberData?: {
            upvotes?: number;
            downvotes?: number;
            userDescription?: string;
            followers?: [];
            following?: [];
            userFavoriteGames?: [];
        }
    }
}