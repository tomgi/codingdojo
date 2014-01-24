require 'ranks/base_rank'

class Flush < BaseRank

	def name
		"flush"
	end

	def self.rank
		6
	end

	def to_s
		"#{name}"
	end
end