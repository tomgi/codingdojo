require 'ranks/base_rank'

class Flush < BaseRank

	def self.rank
		6
	end

	def to_s
		"flush"
	end
end